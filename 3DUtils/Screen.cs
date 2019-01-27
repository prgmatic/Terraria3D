using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Screen
    {
        private static GraphicsDevice _graphicsDevice { get { return Main.graphics.GraphicsDevice; } }
        public static int Width { get { return _graphicsDevice.Viewport.Width; } }
        public static int Height { get { return _graphicsDevice.Viewport.Height; } }
        public static float Aspect { get { return (float)Width / Height; } }
    }
}
