using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria3D.UI.Elements;

namespace Terraria3D
{
	public class UISettingsWindow : UIWindow
	{
		private UIListScrollView _scrollView = new UIListScrollView();
		private UIText _resetButton = new UIText("Reset");
		private UIText _toggleAOButton = new UIText("Toggle AO");


		public UISettingsWindow(string text)
		{
			Width.Set(600, 0);
			Height.Set(-200, 1);
			VAlign = 0.5f;
			HAlign = 0.5f;


			_scrollView.Width.Set(0, 1);
			_scrollView.Height.Set(-30, 1);

			_resetButton.VAlign = 1;
			_resetButton.Top.Set(-5, 0);
			_resetButton.OnClick += (evt, listener) => Resest();

			_toggleAOButton.VAlign = 1;
			_toggleAOButton.Top = _resetButton.Top;
			_toggleAOButton.Left.Pixels = 60;
			_toggleAOButton.OnClick += (evt, listener) => Terraria3D.Instance.Scene.AmbientOcclusion = !Terraria3D.Instance.Scene.AmbientOcclusion;


			Append(_scrollView);
			Append(_resetButton);
			if (Renderers.SM3Enabled)
				Append(_toggleAOButton);
		}

		public override void OnActivate()
		{
			base.OnActivate();
			RebuildLayerList();
		}

		private void RebuildLayerList()
		{
			_scrollView.List.Clear();
			var layers = Terraria3D.Instance.LayerManager.Layers;
			for (int i = 0; i < layers.Length; i++)
				_scrollView.List.Add(new UILayerEntry(i) { Layer = layers[i] });
		}

		private void Resest()
		{
			Terraria3D.Instance.LayerManager.Rebuild();
			RebuildLayerList();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			// Block cursor from clicking world.
			if (ContainsPoint(Main.MouseScreen) && !PlayerInput.IgnoreMouseInterface)
				Main.LocalPlayer.mouseInterface = true;
			base.DrawSelf(spriteBatch);
		}
	}
}