using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Terraria3D
{
    public class Layer3D
    {
        public float ZPos { get; set; }
        public float Depth { get; set; }
        public float NoiseAmount { get; set; }
        public bool UseInnerPixel { get; set; }
        public Action RenderFunction { get; set; }

        private RenderTarget2D _renderTarget;
        private RenderTarget2D _innerPixelTarget;

        public Layer3D()
        {
            UpdateRenderTarget();
            Main.OnResolutionChanged += (size) => UpdateRenderTarget();
            ZPos = 0;
            Depth = 16;
            NoiseAmount = 1;
            UseInnerPixel = true;
        }

        public void RenderToTarget()
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            RenderFunction();
            Main.spriteBatch.End();
            if (UseInnerPixel)
                Renderers.InnerPixelRenderer.Draw(_innerPixelTarget, _renderTarget);
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public void Dispose()
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            if (_innerPixelTarget != null)
                _innerPixelTarget.Dispose();
        }

        public void DrawExtrusion(Camera camera, Matrix matrix)
        {
            matrix = Matrix.CreateScale(1, 1, Depth) * Matrix.CreateTranslation(0, 0, -ZPos) * matrix;
            Renderers.GridRenderer.Draw(UseInnerPixel ? _innerPixelTarget : _renderTarget, camera, Depth, NoiseAmount, matrix);
        }
        public void DrawCap(Camera camera, Matrix matrix)
        {
            matrix = Matrix.CreateTranslation(0, 0, -ZPos) * matrix; //ZPos * 1f / Screen.Height);
            Renderers.CapRenderer.Draw(_renderTarget, camera, matrix);
        }

        private void UpdateRenderTarget()
        {
            Dispose();
            _renderTarget = Utils.CreateRenderTarget();
            _innerPixelTarget = Utils.CreateRenderTarget();
        }
    }
}
