using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Renderers
    {
        public static GridRenderer GridRenderer { get; private set; }
        public static CapRenderer CapRenderer { get; private set; }
        public static InnerPixelRenderer InnerPixelRenderer { get; private set; }

        static Renderers()
        {
            GridRenderer = new GridRenderer(GetEffect("Effects/Grid"), GetTexture("Images/Noise"), Screen.Width, Screen.Height);
            CapRenderer = new CapRenderer(GetEffect("Effects/Texture"));
            InnerPixelRenderer = new InnerPixelRenderer(GetEffect("Effects/InnerPixel"));

            Main.OnResolutionChanged += (size) => GridRenderer.SetGridSize(Screen.Width, Screen.Height);
        }

        private static Effect GetEffect(string name) => Terraria3D.Instance.GetEffect(name);
        private static Texture2D GetTexture(string name) => Terraria3D.Instance.GetTexture(name);
    }
}
