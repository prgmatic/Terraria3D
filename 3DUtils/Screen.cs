using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public static class Screen
    {
        private static GraphicsDevice _graphicsDevice;
        public static int Width => _graphicsDevice.Viewport.Width;
        public static int Height => _graphicsDevice.Viewport.Height;
        public static float Aspect => (float)Width / Height;

        public static void Initialize(GraphicsDevice device)
            => _graphicsDevice = device;
    }
}
