using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using ReLogic.Graphics;
using System.Linq;

namespace Terraria3D
{
    public class Layer3D
    {
        public float ZPos { get; set; } = 0;
        public float Depth { get; set; } = 0;
        public Action RenderFunction { get; set; } = null;

        private RenderTarget2D _renderTarget;

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
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
        }

        public void DrawExtrusion(Camera camera, Matrix matrix)
        {
            //RenderTarget2D prevTarget = Utils.GetCurrentRenderTarget();

            
            //Main.graphics.GraphicsDevice.SetRenderTarget(prevTarget);
            //Main.spriteBatch.Begin();
            //Main.spriteBatch.Draw(target, Vector2.Zero, Color.Purple);
            //Main.spriteBatch.End();
            Renderers.GridRenderer.Draw(_renderTarget, camera, matrix);
        }

        private void UpdateRenderTarget()
        {
            _renderTarget?.Dispose();
            _renderTarget = Utils.CreateRenderTarget();
        }
    }
}
