using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace Terraria3D
{
	public class GenericUserInterface : UserInterface
	{
		public UIState State { get; protected set; }
		public string LayerName { get; private set; }
		public bool Visible { get; set; } = true;

		public GenericUserInterface(string layerName)
		{
			State = CreateState();
			SetState(State);
			State.Activate();
			LayerName = string.Format("{0}: {1}", Terraria3D.Instance.DisplayName, layerName);
		}

		protected virtual UIState CreateState() => new UIState();

		public virtual void Dispose()
		{
			SetState(null);
			State.Deactivate();
			State = null;
		}

		public void UpdateIfVisible(GameTime gameTime)
		{
			if (Visible)
				Update(gameTime);
		}

		public void InsertIntoLayers(string layerName, List<GameInterfaceLayer> layers, InterfaceScaleType scaleType = InterfaceScaleType.UI)
		{
			int inventoryIndex = layers.FindIndex(layer => layer.Name.Equals(layerName));
			if (inventoryIndex != -1)
			{
				layers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(LayerName, () =>
				{
					if (Visible) Draw(Main.spriteBatch, new GameTime());
					return true;
				}, scaleType));
			}
		}
	}
}
