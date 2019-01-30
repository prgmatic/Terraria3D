using Microsoft.Xna.Framework.Graphics;

namespace Terraria3D
{
    public static class Renderers
    {
        public static GridRenderer GridRenderer { get; private set; }
        public static CapRenderer CapRenderer { get; private set; }
        public static InnerPixelRenderer InnerPixelRenderer { get; private set; }

        public static void Load()
        {
            RTManager.Load();
            GridRenderer = new GridRenderer(GetEffect("Effects/Grid"), GetTexture("Images/Noise"), Screen.Width, Screen.Height, RTManager.Width, RTManager.Height);
            CapRenderer = new CapRenderer(GetEffect("Effects/Texture"));
            InnerPixelRenderer = new InnerPixelRenderer(GetEffect("Effects/InnerPixel"));
        }

       
       
        public static void Unload()
        {
            GridRenderer?.Dispose();
            CapRenderer?.Dispose();
            InnerPixelRenderer?.Dispose();

            GridRenderer = null;
            CapRenderer = null;
            InnerPixelRenderer = null;

            RTManager.ResolutionChanged -= ResolutionChanged;
            RTManager.Unload();
        }

        private static Effect GetEffect(string name) => Terraria3D.Instance.GetEffect(name);
        private static Texture2D GetTexture(string name) => Terraria3D.Instance.GetTexture(name);
        private static void ResolutionChanged(int w, int h, int rtW, int rtH) => GridRenderer.SetGridSize(w, h, rtW, rtH);
    }
}
