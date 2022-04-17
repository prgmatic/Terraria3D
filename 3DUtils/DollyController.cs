using System.Threading.Tasks;

namespace Terraria3D;

public class DollyController
{
	const float TRANSITION_TIME = 0.2f;

	public Camera TransitionCamera { get; private set; }
	public bool DollyInProgress => _transition.InProgress;

	private Camera _camera;
	private DollyTransition _transition;

	public DollyController(Camera camera)
	{
		_camera = camera;
		TransitionCamera = new Camera(_camera);
		_transition = new DollyTransition(TransitionCamera);
	}

	public void Update(float deltaTime) => _transition.Update(deltaTime);

	public void TransitionIn()
		=> _transition.TransitionIn(TRANSITION_TIME, _camera);

	public async Task TransitionOutAsync()
		=> await _transition.TransitionOutAsync(TRANSITION_TIME, _camera);
}