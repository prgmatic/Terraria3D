using Terraria;
using Microsoft.Xna.Framework;

namespace Terraria3D
{
    public class Camera
    {
        public Transfrom Transform { get; private set; } = new Transfrom();
        public float FieldOfView { get; set; } = 60;
        public float NearClipPlane { get; set; } = 0.001f;
        public float FarClipPlane { get; set; } = 200f;

		public Camera() { }

		public Camera(Camera sourceCamera)
		{
			FieldOfView = sourceCamera.FieldOfView;
			NearClipPlane = sourceCamera.NearClipPlane;
			FarClipPlane = sourceCamera.FarClipPlane;
		}

		public void LookAt(Vector3 position) => LookAt(position, Vector3.Up);
        public void LookAt(Vector3 position, Vector3 up)
        {
            Transform.Rotation = MathUtils.LookRotation(Transform.Position - position, up);
        }

        public Matrix Projection => Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfView), Screen.Aspect, NearClipPlane, FarClipPlane);
        public Matrix View => Matrix.Invert(Transform.LocalToWorld);
        public Ray ScreenPointToRay(Vector2 screenPosition)
        {
            Vector3 nearPoint = new Vector3(screenPosition, 0); //new Vector3(screenPosition.X, screenPosition.Y, 0);
            Vector3 farPoint  = nearPoint + Vector3.Backward;

            nearPoint = Main.graphics.GraphicsDevice.Viewport.Unproject(nearPoint, Projection, View, Matrix.Identity);
            farPoint  = Main.graphics.GraphicsDevice.Viewport.Unproject(farPoint,  Projection, View, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            return new Ray(nearPoint, direction);
        }
    }
}
