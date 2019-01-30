using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public class PixelGrid
    {
        public static VertexPositionNormalTexture[] Create(int width, int height, int targetWidth, int targetHeight)
        {
            int columns = width + 1;
            int rows = height + 1;

            float maxXUV = (float)width / targetWidth;
            float maxYUV = (float)height / targetHeight;

            int vertCount = columns * 6 + rows * 6;

            var result = new VertexPositionNormalTexture[vertCount];

            int index = 0;
            for (int x = 0; x < columns; x++)
            {
                result[index + 0].Position = new Vector3(x, 0, 0);
                result[index + 1].Position = new Vector3(x, 0, -1);
                result[index + 2].Position = new Vector3(x, height, -1);

                result[index + 3].Position = new Vector3(x, 0, 0);
                result[index + 4].Position = new Vector3(x, height, -1);
                result[index + 5].Position = new Vector3(x, height, 0);
                for (int i = 0; i < 6; i++)
                {
                    result[index + i].Normal = Vector3.Right;
                    float y = result[index + i].Position.Y > 0.0001f ? maxYUV : 0;
                    result[index + i].TextureCoordinate = new Vector2((float)x / targetWidth, maxYUV - y);
                    System.Console.WriteLine(result[index + i].TextureCoordinate);
                }
                index += 6;
            }

            for (int y = 0; y < rows; y++)
            {
                // flip y
                result[index + 0].Position = new Vector3(0, y, 0);
                result[index + 1].Position = new Vector3(0, y, -1);
                result[index + 2].Position = new Vector3(width, y, -1);

                result[index + 3].Position = new Vector3(0, y, 0);
                result[index + 4].Position = new Vector3(width, y, -1);
                result[index + 5].Position = new Vector3(width, y, 0);
                for (int i = 0; i < 6; i++)
                {
                    result[index + i].Normal = Vector3.Down;
                    float x = result[index + i].Position.X > 0.0001f ? maxXUV : 0;
                    result[index + i].TextureCoordinate = new Vector2(x, (float)(height - y) / targetHeight);
                }
                index += 6;
            }
            return result;
        }
    }
}
