using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Screen
    {
        private static GraphicsDevice _graphicsDevice => Main.graphics.GraphicsDevice;
        public static int Width => _graphicsDevice.Viewport.Width;
        public static int Height => _graphicsDevice.Viewport.Height;
        public static float Aspect => (float)Width / Height;

    }
}
