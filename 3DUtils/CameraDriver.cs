using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Terraria;

namespace Terraria3D
{
	public partial class CameraDriver
	{
		public Camera Camera { get; private set; }
		public float LookAtDistance { get; private set; }
		public float MoveSpeed { get; set; } = 1f;
		public float LookSensitivity { get; set; } = 1f;
		public CameraMoveMode MoveMode { get; set; } = CameraMoveMode.Relative;
		public CameraYStart YStartPosition { get; set; } = CameraYStart.Center;

		private bool _moveModeRelative => MoveMode == CameraMoveMode.Relative;

		private Vector3 _forwardAxis => _moveModeRelative ? Camera.Transform.Forward : Vector3.Forward;
		private Vector3 _rightAxis   => _moveModeRelative ? Camera.Transform.Right   : Vector3.Right;
		private Vector3 _upAxis      => _moveModeRelative ? Camera.Transform.Up      : Vector3.Up;

		public CameraDriver(Camera camera)
		{
			Camera = camera;
			ResetCameraPosition();
		}

		public void Drive(float deltaTime)
		{
			var mouseDelta = new Vector2(Main.mouseX - Main.lastMouseX, Main.mouseY - Main.lastMouseY);

			if (Main.keyState.IsKeyDown(_resetKey))
				ResetCameraPosition();

			if (Main.keyState.IsKeyDown(Keys.LeftAlt) && !Main.LocalPlayer.mouseInterface)
			{
				if (Main.mouseLeft)
					MouseOrbit(LookSensitivity * 5f, mouseDelta, deltaTime);
				if (Main.mouseMiddle)
					PanCamera(LookSensitivity * 0.04f, mouseDelta, deltaTime);
				if (Main.mouseRight)
					MouseZoom(LookSensitivity * 0.04f, mouseDelta, deltaTime);
			}
			else
			{
				FlyCamera(MoveSpeed * 0.2f, deltaTime);
				if (!Main.LocalPlayer.mouseInterface)
					Rotate(LookSensitivity * 5f, mouseDelta, deltaTime);
				ArrowKeyOrbit(45, deltaTime);
			}
		}

		private Vector3 GetLookAtPoint() => Camera.Transform.Position + Camera.Transform.Forward * LookAtDistance;

		public enum CameraMoveMode
		{
			Relative,
			World
		}
		public enum CameraYStart
		{
			Center,
			Offset
		}
	}
}