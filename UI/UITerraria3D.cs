using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;

namespace Terraria3D;

public static class UITerraria3D
{
	public static UserInterfaceSettings SettingsInterface { get; private set; }
	public static UserInterfaceOverlay  OverlayInterface  { get; private set; }


	public static void Load()
	{
		SettingsInterface = new UserInterfaceSettings("Settings");
		OverlayInterface = new UserInterfaceOverlay("Overlay");
	}

	public static void Unload()
	{
		SettingsInterface.Dispose();
		OverlayInterface.Dispose();
		SettingsInterface = null;
		OverlayInterface = null;
	}

	public static void Update(GameTime gameTime)
	{
		SettingsInterface.UpdateIfVisible(gameTime);
		OverlayInterface.UpdateIfVisible(gameTime);
	}

	public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		SettingsInterface.InsertIntoLayers("Vanilla: Mouse Text", layers);
		OverlayInterface.InsertIntoLayers("Vanilla: Mouse Text", layers);
	}
}