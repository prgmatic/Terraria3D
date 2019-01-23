using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public class GridRenderer
    {
        private GraphicsDevice _graphicsDevice => Terraria.Main.graphics.GraphicsDevice;

        private Effect _effect;
        private VertexPositionNormalTexture[] _grid;
        private VertexBuffer _gridBuffer;
        private float _width;
        private float _height;

        public GridRenderer(Effect effect, int width, int height)
        {
            _effect = effect;
            _width = width;
            _height = height;

            SetGridSize(width, height);
        }

        public void SetGridSize(int width, int height)
        {
            _grid = PixelGrid.Create(width, height);
            _gridBuffer = new VertexBuffer(_graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, _grid.Length, BufferUsage.WriteOnly);
            _gridBuffer.SetData(_grid);
        }

        public void Draw(Texture texture, Camera camera, Matrix? modelMatrix = null)
        {
            if (modelMatrix == null)
                modelMatrix = Matrix.Identity;

            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);
            _effect.Parameters["_MainTex"].SetValue(texture);
            _effect.Parameters["PixelOffset"].SetValue(new Vector2(1f / _width, 1f / _height));
            _effect.Parameters["World"].SetValue(modelMatrix.Value);

            foreach (var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.SetVertexBuffer(_gridBuffer);
                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _grid.Length / 3);
            }
        }
    }
}