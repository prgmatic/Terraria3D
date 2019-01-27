using Microsoft.Xna.Framework;

namespace Terraria3D
{
    public class Transfrom
    {
        public Vector3 Position    { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale       { get; set; }

        public Matrix LocalToWorld
        {
            get
            {
                return Matrix.CreateScale(Scale) *
                       Matrix.CreateFromQuaternion(Rotation) *
                       Matrix.CreateTranslation(Position);
            }
        }

        public Vector3 Forward { get { return Vector3.Transform(Vector3.Forward, Rotation); } }
        public Vector3 Up { get { return Vector3.Transform(Vector3.Up, Rotation); } }
        public Vector3 Right { get { return Vector3.Transform(Vector3.Right, Rotation); } }

        public Transfrom()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }
    }
}