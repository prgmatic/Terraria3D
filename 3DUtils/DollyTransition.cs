using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Terraria;

namespace Terraria3D
{
	public class DollyTransition
	{
		public bool InProgress { get; private set; } = false;

		private const float Proxy2DFoV = 3;

		private Camera _camera;

		private CameraState _startState;
		private CameraState _targetState;
		private bool _movingIn = false;

		private float _time;
		private float _transitionTime;


		private TaskCompletionSource<bool> _transitionTCS;


		public DollyTransition(Camera camera)
		{
			_camera = camera;
		}
		private float DistFromFov(float fov, float height)
		{
			return height * 0.5f / (float)Math.Tan(fov * 0.5f);
		}

		private CameraState Get2DProxyState()
		{
			var result = new CameraState();
			result.FieldOfView = MathHelper.ToRadians(Proxy2DFoV);
			result.Position = Vector3.Backward * DistFromFov(result.FieldOfView, 1f / Main.GameZoomTarget);
			result.Rotation = Quaternion.Identity;
			return result;
		}

		public async Task TransitionOutAsync(float transitionTime, Camera sceneCamera)
		{
			TransitionOut(transitionTime, sceneCamera);
			await DoTransitionAsync();
		}

		public void TransitionIn(float transitionTime, Camera sceneCamera)
		{
			if (InProgress) return;
			 _movingIn = true;
			SetupTransition(transitionTime);
			_startState = Get2DProxyState();
			_targetState = new CameraState(sceneCamera);
			UpdateCamera(0);
		}

		public void TransitionOut(float transitionTime, Camera sceneCamera)
		{
			if (InProgress) return;
			 _movingIn = false;
			SetupTransition(transitionTime);
			_startState = new CameraState(sceneCamera);
			_targetState = Get2DProxyState();
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
			var fov = MathHelper.Lerp(_startState.FieldOfView, _targetState.FieldOfView, t);
			_camera.FieldOfView = MathHelper.ToDegrees(fov);

			var zPlane = Vector3.Backward * DistFromFov((_movingIn ? _targetState : _startState).FieldOfView, 1f / Main.GameZoomTarget);
			var offset = _movingIn ? Vector3.Lerp(zPlane, _targetState.Position, t) - zPlane :
									 Vector3.Lerp(_startState.Position, zPlane, t) - zPlane;
			_camera.Transform.Position = Vector3.Backward * DistFromFov(fov, 1f / Main.GameZoomTarget) + offset;

			var t2 = _movingIn ? CircleEaseIn(t) : CircleEaseOut(t);
			_camera.Transform.Rotation = Quaternion.Lerp(_startState.Rotation, _targetState.Rotation, t2);
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

		private struct CameraState
		{
			public Vector3 Position { get; set; }
			public Quaternion Rotation { get; set; }
			public float FieldOfView { get; set; }

			public CameraState(Camera camera)
			{
				Position = camera.Transform.Position;
				Rotation = camera.Transform.Rotation;
				FieldOfView = MathHelper.ToRadians(camera.FieldOfView);
			}
		}
	}
}