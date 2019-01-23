
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public class ExtrudeTest
    {
        private GraphicsDevice _graphicsDevice => Terraria.Main.graphics.GraphicsDevice;

        private Effect _gridEffect;
        private VertexPositionNormalTexture[] _grid;
        private VertexBuffer _gridBuffer;
        private Transfrom _pixelGridTrasnform = new Transfrom();
        private float _width;
        private float _height;

        Camera _camera;


        public ExtrudeTest(Effect gridEffect, int width, int height)
        {
            _width = width;
            _height = height;
            _gridEffect = gridEffect;
            _camera = new Camera();
            _camera.Transfrom.Position = Vector3.Backward * 5;

            _grid = PixelGrid.Create(width, height);
            _pixelGridTrasnform.Scale = Vector3.One / height;
            _pixelGridTrasnform.Position = new Vector3(-width * 0.5f, -height * 0.5f, 0) * _pixelGridTrasnform.Scale.X;
            var scale = _pixelGridTrasnform.Scale;
            scale.Z *= 10;
            _pixelGridTrasnform.Scale = scale;
            _gridBuffer = new VertexBuffer(_graphicsDevice, VertexPositionNormalTexture.VertexDeclaration, _grid.Length, BufferUsage.WriteOnly);
            _gridBuffer.SetData(_grid);
        }

        public void Update(float deltaTime)
        {
            CameraDriver.Drive(_camera, 2, 3, deltaTime);
        }

        public void Draw(GraphicsDevice graphicsDevice, Texture texture, Matrix? matrix = null)
        {
            if (matrix == null)
                matrix = Matrix.Identity;

            _gridEffect.Parameters["View"].SetValue(_camera.View);
            _gridEffect.Parameters["Projection"].SetValue(_camera.Projection);
            _gridEffect.Parameters["_MainTex"].SetValue(texture);
            _gridEffect.Parameters["PixelOffset"].SetValue(new Vector2(1f / _width, 1f / _height));
            _gridEffect.Parameters["World"].SetValue(matrix.Value * _pixelGridTrasnform.LocalToWorld);

            foreach (var pass in _gridEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.SetVertexBuffer(_gridBuffer);
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, _grid.Length / 3);
            }
        }
    }
}
