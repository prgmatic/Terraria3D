using Microsoft.Xna.Framework;

namespace Terraria3D;

public class Transform
{
    public Vector3 Position    { get; set; } = Vector3.Zero;
    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    public Vector3 Scale       { get; set; } = Vector3.One;

    public Matrix LocalToWorld => Matrix.CreateScale(Scale) *
                                  Matrix.CreateFromQuaternion(Rotation) *
                                  Matrix.CreateTranslation(Position);

    public Vector3 Forward => Vector3.Transform(Vector3.Forward, Rotation);
    public Vector3 Up => Vector3.Transform(Vector3.Up, Rotation);
    public Vector3 Right => Vector3.Transform(Vector3.Right, Rotation);
}