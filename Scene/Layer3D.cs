using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Terraria3D
{
    public class Layer3D
    {
        public float ZPos { get; set; } = 0;
        public float Depth { get; set; } = 16;
        public float NoiseAmount { get; set; } = 1;
        public bool UseInnerPixel { get; set; } = true;
        public Action RenderFunction { get; set; } = null;

        private RenderTarget2D _renderTarget;
        private RenderTarget2D _innerPixelTarget;

        public Layer3D()
        {
            UpdateRenderTarget();
            Main.OnResolutionChanged += (size) => UpdateRenderTarget();
        }

        public void RenderToTarget()
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(_renderTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
            RenderFunction();
            Main.spriteBatch.End();
            if(UseInnerPixel)
                Renderers.InnerPixelRenderer.Draw(_innerPixelTarget, _renderTarget);
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public void Dispose()
        {
            _renderTarget?.Dispose();
            _innerPixelTarget?.Dispose();
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
