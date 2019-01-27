using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Utils
    {
        public static RenderTarget2D CreateRenderTarget(bool preserve = false) 
            => CreateRenderTarget(Screen.Width, Screen.Height, preserve);


        public static RenderTarget2D CreateRenderTarget(int width, int height, bool preserve = false)
        {
            var g = Main.graphics.GraphicsDevice;
            return new RenderTarget2D(g, width, height, false, g.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 0, preserve ? RenderTargetUsage.PreserveContents : RenderTargetUsage.DiscardContents);
        }

        public static RenderTarget2D GetCurrentRenderTarget()
        {
            RenderTarget2D result = null;
            var bindings = Main.graphics.GraphicsDevice.GetRenderTargets();
            if (bindings.Length > 0)
                result = bindings[0].RenderTarget as RenderTarget2D;
            return result;
        }
    }
}
