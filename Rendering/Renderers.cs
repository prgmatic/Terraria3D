using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Renderers
    {
        public static GridRenderer GridRenderer { get; private set; }
        public static CapRenderer CapRenderer { get; private set; }
        public static InnerPixelRenderer InnerPixelRenderer { get; private set; }
       
        public static void Load()
        {
            GridRenderer = new GridRenderer(GetEffect("Effects/Grid"), GetTexture("Images/Noise"), Screen.Width, Screen.Height);
            CapRenderer = new CapRenderer(GetEffect("Effects/Texture"));
            InnerPixelRenderer = new InnerPixelRenderer(GetEffect("Effects/InnerPixel"));

            Main.OnResolutionChanged += ResolutionChanged; 
        }

       
        public static void Unload()
        {
            GridRenderer?.Dispose();
            CapRenderer?.Dispose();
            InnerPixelRenderer?.Dispose();

            GridRenderer = null;
            CapRenderer = null;
            InnerPixelRenderer = null;

            Main.OnResolutionChanged -= ResolutionChanged;
        }

        private static Effect GetEffect(string name) => Terraria3D.Instance.GetEffect(name);
        private static Texture2D GetTexture(string name) => Terraria3D.Instance.GetTexture(name);
        private static void ResolutionChanged(Vector2 size) => GridRenderer.SetGridSize(Screen.Width, Screen.Height);
    }
}
