using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public class CapRenderer
    {
        private GraphicsDevice _graphicsDevice => Terraria.Main.graphics.GraphicsDevice;

        private Effect _effect;
        private VertexBuffer _buffer;

        public CapRenderer(Effect effect)
        {
            _effect = effect;
            var quad = Quad.Create();
            _buffer = new VertexBuffer(_graphicsDevice, VertexPositionTexture.VertexDeclaration, quad.Length, BufferUsage.WriteOnly);
            _buffer.SetData(quad);
        }

        public void Dispose()
        {
            _effect?.Dispose();
            _buffer?.Dispose();
        }

        public void Draw(Texture texture, Camera camera,  Matrix matrix)
        {
            _effect.Parameters["World"].SetValue(matrix);
            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);
            _effect.Parameters["_MainTex"].SetValue(texture);

            foreach(var pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.SetVertexBuffer(_buffer);
                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }
    }
}
