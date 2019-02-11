using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Terraria;

namespace Terraria3D
{
	public class DollyTransition
	{
		private Camera _camera;

		private float _startFov;
		private float _targetFov;
		private Vector3 _startPos;
		private Quaternion _startRot;
		private Vector3 _targetPos;
		private Quaternion _targetRot;
		private bool _movingIn = false;

		private float _time;
		private float _transitionTime;

		private TaskCompletionSource<bool> _transitionTCS;

		public bool InProgress { get; private set; } = false;

		public DollyTransition(Camera camera)
		{
			_camera = camera;
		}
		private float DistFromFov(float fov, float height)
		{
			return height * 0.5f / (float)Math.Tan(fov * 0.5f);
		}

		public async Task TransitionInAsync(float transitionTime, float startFov, float targetFov, Vector3 targetPos, Quaternion targetRot)
		{
			TransitionIn(transitionTime, startFov, targetFov, targetPos, targetRot);
			await DoTransitionAsync();
		}

		public async Task TransitionOutAsync(float transitionTime, float targetFov)
		{
			TransitionOut(transitionTime, targetFov);
			await DoTransitionAsync();
		}

		public void TransitionIn(float transitionTime, float startFov, float targetFov, Vector3 targetPos, Quaternion targetRot)
		{
			if (InProgress) return;
			 _movingIn = true;
			_camera.FieldOfView = startFov;
			_camera.Transform.Position = Vector3.Backward * DistFromFov(_startFov, 1f / Main.GameZoomTarget);
			SetupTransition(transitionTime);
			_targetFov = MathHelper.ToRadians(targetFov);
			_targetPos = targetPos;
			_targetRot = targetRot;
			UpdateCamera(0);
		}

		public void TransitionOut(float transitionTime, float targetFov)
		{
			if (InProgress) return;
			 _movingIn = false;
			SetupTransition(transitionTime);
			_targetFov = MathHelper.ToRadians(targetFov);
			_targetPos = Vector3.Backward * DistFromFov(MathHelper.ToRadians(_camera.FieldOfView), 1f / Main.GameZoomTarget);
			_targetRot = Quaternion.Identity;
		}

		private async Task DoTransitionAsync()
		{
			_transitionTCS = new TaskCompletionSource<bool>();
			await _transitionTCS.Task;
		}

		private void SetupTransition(float transitionTime)
		{
			InProgress = true;
			_transitionTime = transitionTime;
			_time = 0;
			_startFov = MathHelper.ToRadians(_camera.FieldOfView);
			_startPos = _camera.Transform.Position;
			_startRot = _camera.Transform.Rotation;
		}

		private float CircleEaseOut(float t)
		{
			t -= 1;
			return (float)Math.Sqrt(1 - t * t);
		}
		private float CircleEaseIn(float t)
		{
			return 1 - (float)Math.Sqrt(1 - t * t);
		}

		private void UpdateCamera(float t)
		{
			t = _movingIn ? CircleEaseOut(t) : CircleEaseIn(t);
			var fov = MathHelper.Lerp(_startFov, _targetFov, t);
			_camera.FieldOfView = MathHelper.ToDegrees(fov);

			var offset = Vector3.Lerp(_startPos, _targetPos, t) - (_movingIn ? _startPos : _targetPos);
			_camera.Transform.Position = Vector3.Backward * DistFromFov(fov, 1f / Main.GameZoomTarget) + offset;

			if (_movingIn)
				t = (t - 0.5f) * 2;
			else
				t *= 2;
			t = MathHelper.Clamp(t, 0, 1);
			var t2 = _movingIn ? CircleEaseIn(t) : CircleEaseOut(t);
			_camera.Transform.Rotation = Quaternion.Lerp(_startRot, _targetRot, t);
		}

		public void Update(float deltaTime)
		{
			if(InProgress)
			{
				var t = _time / _transitionTime;
				UpdateCamera(t);
				_time += deltaTime;
				if (_time >= _transitionTime)
				{
					InProgress = false;
					UpdateCamera(1);
					_transitionTCS?.SetResult(true);
					_transitionTCS = null;
				}
			}
		}
	}
}