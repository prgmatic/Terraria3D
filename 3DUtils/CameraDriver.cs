using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.GameInput;

namespace Terraria3D
{
	public class CameraDriver
	{
		private static AxisBindings _forwardAxisInput = new AxisBindings("Up", "Down");
		private static AxisBindings _rightAxisInput = new AxisBindings("Right", "Left");
		private static AxisKeys _upAxisInput = new AxisKeys(Keys.E, Keys.Q);
		private static Keys _resetKey = Keys.F;

		public Camera Camera { get; private set; }
		public float LookAtDistance { get; private set; }
		public float MoveSpeed { get; set; } = 1f;
		public float LookSensitivity { get; set; } = 1f;
		public bool CameraRelativeMode { get; set; } = true;
		public bool CameraStartYOffset { get; set; } = true;

		private Vector3 _forwardAxis => CameraRelativeMode ? Camera.Transform.Forward : Vector3.Forward;
		private Vector3 _rightAxis => CameraRelativeMode ? Camera.Transform.Right : Vector3.Right;
		private Vector3 _upAxis => CameraRelativeMode ? Camera.Transform.Up : Vector3.Up;

		public CameraDriver(Camera camera)
		{
			Camera = camera;
			ResetCameraPosition();
		}

		public void ResetCameraPosition()
		{
			Camera.Transform.Position = CameraStartYOffset ? new Vector3(0, 0.08f, 0.4f) : Vector3.Backward * 0.4f;
			Camera.LookAt(Vector3.Zero, Vector3.Up);
			LookAtDistance = Vector3.Distance(Camera.Transform.Position, Vector3.Zero);
		}

		public void Drive(float deltaTime)
		{
			var mouseDelta = new Vector2(Main.mouseX - Main.lastMouseX, Main.mouseY - Main.lastMouseY);

			if (Main.keyState.IsKeyDown(_resetKey))
				ResetCameraPosition();

			if (Main.keyState.IsKeyDown(Keys.LeftAlt))
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
				Rotate(LookSensitivity * 5f, mouseDelta, deltaTime);
				ArrowKeyOrbit(45, deltaTime);
			}
		}

		private void FlyCamera(float moveSpeed, float deltaTime)
		{
			var moveVector = _forwardAxis * _forwardAxisInput.GetValue() * moveSpeed +
				             _rightAxis   * _rightAxisInput  .GetValue() * moveSpeed +
				             _upAxis      * _upAxisInput     .GetValue() * moveSpeed;
			Move(moveVector, deltaTime);
		}

		private void PanCamera(float sensitivity, Vector2 mouseDelta, float deltaTime)
		{
			var moveVector = _rightAxis * -mouseDelta.X * sensitivity +
							 _upAxis    *  mouseDelta.Y * sensitivity;
			Move(moveVector, deltaTime);
		}

		private void Move(Vector3 moveVector, float deltaTime)
			=>Camera.Transform.Position += moveVector * deltaTime;

		private void MouseOrbit(float sensitivity, Vector2 mouseDelta, float deltaTime)
		{
			OrbitX(-mouseDelta.Y * sensitivity, deltaTime);
			OrbitY(-mouseDelta.X * sensitivity, deltaTime);
		}

		private void Rotate(float lookSensitivity, Vector2 mouseDelta, float deltaTime)
		{
			if (Main.mouseLeft)
			{
				Camera.Transform.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-mouseDelta.X * lookSensitivity) * deltaTime) * Camera.Transform.Rotation;
				Camera.Transform.Rotation = Quaternion.CreateFromAxisAngle(Camera.Transform.Right, MathHelper.ToRadians(-mouseDelta.Y * lookSensitivity) * deltaTime) * Camera.Transform.Rotation;
			}
		}

		private void ArrowKeyOrbit(float degreesPerSecond, float deltaTime)
		{
			var yRot = 0f;
			if (Main.keyState.IsKeyDown(Keys.Left)) yRot -= degreesPerSecond;
			if (Main.keyState.IsKeyDown(Keys.Right)) yRot += degreesPerSecond;

			var xRot = 0f;
			if (Main.keyState.IsKeyDown(Keys.Up)) xRot -= degreesPerSecond;
			if (Main.keyState.IsKeyDown(Keys.Down)) xRot += degreesPerSecond;

			OrbitY(yRot, deltaTime);
			OrbitX(xRot, deltaTime);
		}

		private void OrbitY(float angle, float deltaTime) => Orbit(angle, Vector3.Up            , deltaTime);
		private void OrbitX(float angle, float deltaTime) => Orbit(angle, Camera.Transform.Right, deltaTime);

		private void Orbit(float angle, Vector3 axis, float deltaTime)
		{
			var lookAtPoint = GetLookAtPoint();
			var matrix = GetOrbitMatrix(lookAtPoint, Matrix.CreateFromAxisAngle(axis, MathHelper.ToRadians(angle * deltaTime)));
			Camera.Transform.Position = Vector3.Transform(Camera.Transform.Position, matrix);
			Camera.LookAt(lookAtPoint, Vector3.Up);
		}

		private Matrix GetOrbitMatrix(Vector3 center, Matrix rotation)
		{
			return Matrix.CreateTranslation(-center) *
					rotation *
					  Matrix.CreateTranslation(center);
		}

		private void MouseZoom(float sensitivity, Vector2 mouseDelta, float deltaTime)
		{
			var distance = (mouseDelta.X + mouseDelta.Y) * sensitivity;
			Zoom(distance, deltaTime);
		}

		// This doesn't change the FOV. It moves the camera along
		// the forward axis while keeping the look at point stationary.
		private void Zoom(float distance, float deltaTime)
		{
			var minDistFromLookAtPoint = 0.1f;
			var maxDistFromLookAtPoint = 2f;

			var lookAtPoint = GetLookAtPoint();
			Camera.Transform.Position += Camera.Transform.Forward * distance * deltaTime;
			LookAtDistance = Vector3.Distance(Camera.Transform.Position, lookAtPoint);
			// Check if camera shot past the look at point.
			if (Vector3.Dot(Camera.Transform.Forward, Camera.Transform.Position - lookAtPoint) > 0)
				Camera.Transform.Position = lookAtPoint - Camera.Transform.Forward * minDistFromLookAtPoint;
			else
			{
				LookAtDistance = MathHelper.Clamp(LookAtDistance, minDistFromLookAtPoint, maxDistFromLookAtPoint);
				Camera.Transform.Position = lookAtPoint - Camera.Transform.Forward * LookAtDistance;
			}
		}

		private Vector3 GetLookAtPoint() => Camera.Transform.Position + Camera.Transform.Forward * LookAtDistance;

		abstract class Axis<T>
		{
			public T Positive { get; set; }
			public T Negative { get; set; }

			public Axis(T positive, T negative)
			{
				Positive = positive;
				Negative = negative;
			}
			public abstract float GetValue();
		}

		private class AxisBindings : Axis<string>
		{
			public AxisBindings(string positive, string negative) : base(positive, negative) { }

			public override float GetValue()
			{
				float result = 0;
				if (PlayerInput.Triggers.Current.KeyStatus[Negative]) result -= 1;
				if (PlayerInput.Triggers.Current.KeyStatus[Positive]) result += 1;
				return result;
			}
		}

		private class AxisKeys : Axis<Keys>
		{
			public AxisKeys(Keys positive, Keys negative) : base(positive, negative) { }

			public override float GetValue()
			{
				float result = 0;
				if (Main.keyState.IsKeyDown(Negative)) result -= 1;
				if (Main.keyState.IsKeyDown(Positive)) result += 1;
				return result;
			}
		}
	}
}