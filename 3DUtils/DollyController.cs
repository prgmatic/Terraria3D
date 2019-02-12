
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace Terraria3D
{
	public class DollyController
	{
		const float TRANSITION_TIME = 0.2f;

		public bool DollyInProgress => _transition.InProgress;

		private Camera _camera;
		private DollyTransition _transition;
		private Vector3 _enterTargetPos;
		private Quaternion _enterTargetRot;
		private float _entertargetFov;

		public DollyController(Camera camera)
		{
			_camera = camera;
			_transition = new DollyTransition(camera);
			UpdateEnterPos();
		}

		public void Update(float deltaTime)
		{
			_transition.Update(deltaTime);
		}

		public void TransitionIn()
		{
			_transition.TransitionIn(TRANSITION_TIME, 3, _entertargetFov, _enterTargetPos, _enterTargetRot);
		}
		public async Task TransitionOutAsync()
		{
			UpdateEnterPos();
			await _transition.TransitionOutAsync(TRANSITION_TIME, 3);
		}

		private void UpdateEnterPos()
		{
			_enterTargetPos = _camera.Transform.Position;
			_enterTargetRot = _camera.Transform.Rotation;
			_entertargetFov = _camera.FieldOfView;
		}
	}
}
