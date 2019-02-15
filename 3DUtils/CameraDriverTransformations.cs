using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace Terraria3D
{
	public partial class CameraDriver
	{
		public void ResetCameraPosition()
		{
			Camera.Transform.Position = YStartPosition == CameraYStart.Offset ? new Vector3(0, 0.08f, 0.4f) : Vector3.Backward * 0.4f;
			Camera.LookAt(Vector3.Zero, Vector3.Up);
			Camera.FieldOfView = 60;
			LookAtDistance = Vector3.Distance(Camera.Transform.Position, Vector3.Zero);
		}

		private void FlyCamera(float moveSpeed, float deltaTime)
		{
			var moveVector = _forwardAxis * _forwardAxisInput.GetValue() * moveSpeed +
							 _rightAxis * _rightAxisInput.GetValue() * moveSpeed +
							 _upAxis * _upAxisInput.GetValue() * moveSpeed;
			Move(moveVector, deltaTime);
		}

		private void PanCamera(float sensitivity, Vector2 mouseDelta, float deltaTime)
		{
			var moveVector = _rightAxis * -mouseDelta.X * sensitivity +
							 _upAxis * mouseDelta.Y * sensitivity;
			Move(moveVector, deltaTime);
		}

		private void Move(Vector3 moveVector, float deltaTime)
			=> Camera.Transform.Position += moveVector * deltaTime;

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

		private void OrbitY(float angle, float deltaTime) => Orbit(angle, Vector3.Up, deltaTime);
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

	}
}
