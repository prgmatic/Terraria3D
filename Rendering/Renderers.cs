using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Renderers
    {
        public static GridRenderer GridRenderer { get; private set; }
        public static CapRenderer CapRenderer { get; private set; }
        public static RenderTarget2D ScreenTarget { get; private set; }

        static Renderers()
        {
            GridRenderer = new GridRenderer(GetEffect("Effects/Grid"), Main.instance.tileTarget.Width, Main.instance.tileTarget.Height);
            CapRenderer = new CapRenderer(GetEffect("Effects/Texture"));
            SetScreenTargetSize();
            Main.OnResolutionChanged += (size) => SetScreenTargetSize();
            Main.OnRenderTargetsInitialized += (w, h) => SetScreenTargetSize();
        }

        private static Effect GetEffect(string name) => Terraria3D.Instance.GetEffect(name);
        private static void SetScreenTargetSize()
        {
            if (ScreenTarget != null)
                ScreenTarget.Dispose();
            ScreenTarget = Utils.CreateRenderTarget(Screen.Width, Screen.Height, true);
        }
    }
}
