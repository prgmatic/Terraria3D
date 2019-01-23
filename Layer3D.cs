using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Terraria3D
{
    public class Layer3D
    {
        public float ZPos { get; set; } = 0;
        public float Depth { get; set; } = 0;
        public Action RenderFunction { get; set; } = null;

        public void DrawExtrusion(GridRenderer renderer, RenderTarget2D target, Transfrom transform)
        {
            Main.graphics.GraphicsDevice.SetRenderTarget(target);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);
            RenderFunction();
            Main.graphics.GraphicsDevice.SetRenderTarget(null);
            renderer.Draw(target, transform.LocalToWorld);
        }
    }
}
