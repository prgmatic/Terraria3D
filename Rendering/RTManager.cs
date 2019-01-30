using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public static class RTManager
    {
        // I broke NPoT when implementing PoT, may revisit.
        private const bool FORCE_POT = false;

        public delegate void ResolutionChangedEvent(int width, int height, int rtWidth, int rtHeight);
        public static event ResolutionChangedEvent ResolutionChanged;

        public static bool UsingPoT { get; private set; } = true;
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        

        public static void Load()
        {
            UsingPoT = (FORCE_POT || Reflection.CurrentGraphicsProfile == GraphicsProfile.Reach);

            UpdateDimensions();
            Terraria.Main.OnResolutionChanged += MainResolutionChanged;
        }

        public static void Unload()
            => Terraria.Main.OnResolutionChanged -= MainResolutionChanged;

        private static void UpdateDimensions()
        {
            Width = Screen.Width;
            Height = Screen.Height;
            if (UsingPoT)
            {
                Width = Utils.GetSmallestPoT(Width);
                Height = Utils.GetSmallestPoT(Height);
            }
        }

        private static void MainResolutionChanged(Vector2 size)
        {
            UpdateDimensions();
            ResolutionChanged?.Invoke(Screen.Width, Screen.Height, Width, Height);
        }
    }
}
