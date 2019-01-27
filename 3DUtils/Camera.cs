using Microsoft.Xna.Framework;

namespace Terraria3D
{
    public class Camera
    {
        public Transfrom Transfrom { get; private set; }
        public float FieldOfView { get; private set; }
        public float NearClipPlane { get; private set; }
        public float FarClipPlane { get; private set; }

        public Camera()
        {
            Transfrom = new Transfrom();
            FieldOfView = 60;
            NearClipPlane = 0.001f;
            FarClipPlane = 200f;
        }

        public void LookAt(Vector3 position, Vector3 up)
        {
            Transfrom.Rotation = MathUtils.LookRotation(position - Transfrom.Position, up);
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfView), Screen.Aspect, NearClipPlane, FarClipPlane); }
        }
        public Matrix View
        {
            get { return Matrix.Invert(Transfrom.LocalToWorld); }
        }
    }
}
