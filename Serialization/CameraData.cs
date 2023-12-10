using Microsoft.Xna.Framework;

namespace Terraria3D.Serialization;

public class CameraData
{
	public Vector3 Position { get; set; }
	public Quaternion Rotation { get; set; }
	public float FieldOfView { get; set; }
	public CameraDriver.CameraYStart YStartPosition { get; set; }
	public CameraDriver.CameraMoveMode MoveMode { get; set; }

	public void Apply(Scene3D scene)
	{
		scene.Camera.Transform.Position = Position;
		scene.Camera.Transform.Rotation = Rotation;
		scene.Camera.FieldOfView = MathHelper.Clamp(FieldOfView, 3, 179);
		scene.CameraDriver.YStartPosition = YStartPosition;
		scene.CameraDriver.MoveMode = MoveMode;
	}

	public void Record(Scene3D scene)
	{
		Position = scene.Camera.Transform.Position;
		Rotation = scene.Camera.Transform.Rotation;
		FieldOfView = scene.Camera.FieldOfView;
		YStartPosition = scene.CameraDriver.YStartPosition;
		MoveMode = scene.CameraDriver.MoveMode;
	}
}