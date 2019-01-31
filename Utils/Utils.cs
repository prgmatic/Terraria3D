using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace Terraria3D
{
    public static class Utils
    {
        public static RenderTarget2D CreateRenderTarget(bool PoT = false) 
            => CreateRenderTarget(Screen.Width, Screen.Height, PoT);

        // PoT: Make power of two
        public static RenderTarget2D CreateRenderTarget(int width, int height, bool PoT = false)
        {
            if(PoT)
            {
                width = GetSmallestPoT(width);
                height = GetSmallestPoT(height);
            }
            var g = Main.graphics.GraphicsDevice;
            return new RenderTarget2D(g, width, height, false, g.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);
        }

        public static int GetSmallestPoT(int value)
        {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log(value, 2)));
        }

        public static RenderTarget2D GetCurrentRenderTarget()
        {
            RenderTarget2D result = null;
            var bindings = Main.graphics.GraphicsDevice.GetRenderTargets();
            if (bindings.Length > 0)
                result = bindings[0].RenderTarget as RenderTarget2D;
            return result;
        }

        public static void SetZoom(float zoom)
        {
            Main.GameZoomTarget = zoom;
            UpdateGameViewMatrixZoom();
        }
        public static void UpdateGameViewMatrixZoom()
            => Main.GameViewMatrix.Zoom = new Vector2(Main.ForcedMinimumZoom * MathHelper.Clamp(Main.GameZoomTarget, 1f, 2f));
    }
}
