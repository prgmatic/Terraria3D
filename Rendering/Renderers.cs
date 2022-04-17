using System;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace Terraria3D
{
    public static class Renderers
    {
        public static GridRenderer GridRenderer { get; private set; }
        public static CapRenderer CapRenderer { get; private set; }
        public static InnerPixelRenderer InnerPixelRenderer { get; private set; }
		public static bool SM3Enabled => Main.graphics.GraphicsProfile == GraphicsProfile.HiDef;

		private static Effect _gridEffect => SM3Enabled ? GetEffect("Terraria3D/Effects/HiDef/Grid") : GetEffect("Terraria3D/Effects/Grid");


		public static void Load()
        {
            RTManager.Load();
            GridRenderer = new GridRenderer(_gridEffect, GetTexture("Terraria3D/Images/Noise"), Screen.Width, Screen.Height, RTManager.Width, RTManager.Height);
            CapRenderer = new CapRenderer(GetEffect("Terraria3D/Effects/Texture"));
            InnerPixelRenderer = new InnerPixelRenderer(GetEffect("Terraria3D/Effects/InnerPixel"));
            RTManager.ResolutionChanged += ResolutionChanged;
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

        private static Effect GetEffect(string name) 
            => ModContent.Request<Effect>(name, AssetRequestMode.ImmediateLoad).Value;
        private static Texture2D GetTexture(string name) 
            => ModContent.Request<Texture2D>(name, AssetRequestMode.ImmediateLoad).Value;
        private static void ResolutionChanged(int w, int h, int rtW, int rtH) => GridRenderer.SetGridSize(w, h, rtW, rtH);
    }
}
