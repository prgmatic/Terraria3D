
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.UI;

namespace Terraria3D;

public static class InterfaceRendering
{
	public static bool Drawing3D { get; private set; } = false;

	public static void RenderGameInterfaces()
	{
		 if (Main.hideUI) return;
		 Drawing3D = true;
		 Main.spriteBatch.End();
		 var layers = Reflection.GameInterfaceLayers;
		 if (layers != null)
		 {
		 	layers = new List<GameInterfaceLayer>(Reflection.GameInterfaceLayers);
		 	foreach (var layer in layers.Where(i => i.ScaleType == InterfaceScaleType.Game)
		 		         .Where(i => i.Name != "Vanilla: Mouse Over" &&
		 		                     i.Name != "Vanilla: Interface Logic 4"))
		 	{
		 		layer.Draw();
		 	}
		 }
		 Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);
		 Drawing3D = false;
	}
}