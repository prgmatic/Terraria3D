using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace Terraria3D
{
    public static class Renderers
    {
        public static GridRenderer GridRenderer { get; private set; }

        static Renderers()
        {
            GridRenderer = new GridRenderer(GetEffect("Grid"), Main.instance.tileTarget.Width, Main.instance.tileTarget.Height);
        }


        private static Effect GetEffect(string name) => Terraria3D.Instance.GetEffect(name);
    }
}
