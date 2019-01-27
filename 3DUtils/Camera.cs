using Microsoft.Xna.Framework;

namespace Terraria3D
{
    public class Camera
    {
        public Transfrom Transfrom { get; private set; } = new Transfrom();
        public float FieldOfView { get; private set; } = 60;
        public float NearClipPlane { get; private set; } = 0.001f;
        public float FarClipPlane { get; private set; } = 200f;
        

        public void LookAt(Vector3 position, Vector3 up)
        {
            Transfrom.Rotation = MathUtils.LookRotation(position - Transfrom.Position, up);
        }

        public Matrix Projection => Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FieldOfView), Screen.Aspect, NearClipPlane, FarClipPlane);
        public Matrix View => Matrix.Invert(Transfrom.LocalToWorld);
    }
}
