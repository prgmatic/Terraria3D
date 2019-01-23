using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public class Quad
    {
        public static VertexPositionTexture[] Create()
        {
            var result = new VertexPositionTexture[6];

            result[0].Position = new Vector3(-0.5f, -0.5f, 0);
            result[1].Position = new Vector3(-0.5f, 0.5f, 0);
            result[2].Position = new Vector3(0.5f, 0.5f, 0);

            result[3].Position = new Vector3(-0.5f, -0.5f, 0);
            result[4].Position = new Vector3(0.5f, 0.5f, 0);
            result[5].Position = new Vector3(0.5f, -0.5f, 0);

            result[0].TextureCoordinate = new Vector2(0, 1);
            result[1].TextureCoordinate = new Vector2(0, 0);
            result[2].TextureCoordinate = new Vector2(1, 0);

            result[3].TextureCoordinate = new Vector2(0, 1);
            result[4].TextureCoordinate = new Vector2(1, 0);
            result[5].TextureCoordinate = new Vector2(1, 1);

            return result;
        }
    }
}
