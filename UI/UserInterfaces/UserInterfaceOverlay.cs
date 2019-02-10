using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria3D
{
	public class UserInterfaceOverlay : GenericUserInterface
	{
		public UserInterfaceOverlay(string layerName) : base(layerName) { State.Recalculate(); }
		protected override UIState CreateState() => new OverlayState();

		public override void Dispose()
		{
			base.Dispose();
			(State as OverlayState)?.Dispose();
		}
	}

	class OverlayState : UIState
	{
		private static Texture2D _cameraIcon => Terraria3D.Instance.GetTexture("Images/CameraIcon");

		private UIImage _editCameraIcon = new UIImage(_cameraIcon);
		private UICameraControlsPanel _controlsPanel = new UICameraControlsPanel();
		private UIText _editCameraHoverTip = new UIText("Edit Camera");
		private UITextPanel<string> _toggleControlsPanelButton = new UITextPanel<string>("Hide Controls");

		public OverlayState() : base()
		{
			_editCameraIcon.Recalculate();
			_editCameraIcon.VAlign = 1;
			_editCameraIcon.Left.Pixels = 20;
			_editCameraIcon.Top.Pixels = -20;
			_editCameraIcon.OnClick += (evt, listener) =>
				InputTerraria3D.CameraControlsEnabled = !InputTerraria3D.CameraControlsEnabled;

			_controlsPanel.VAlign = 1;
			_controlsPanel.Left.Pixels = 20;
			_controlsPanel.Top.Pixels = -100;

			_editCameraHoverTip.VAlign = 1;
			_editCameraHoverTip.Left.Pixels = 20;
			_editCameraHoverTip.Top.Pixels = -70;

			_toggleControlsPanelButton.VAlign = 1;
			_toggleControlsPanelButton.Left.Pixels = 80;
			_toggleControlsPanelButton.Top.Pixels = -15;
			_toggleControlsPanelButton.OnClick += (evt, listener) =>
			{
				if (InputTerraria3D.CameraControlsEnabled)
				{
					_controlsPanel.Visible = !_controlsPanel.Visible;
					_toggleControlsPanelButton.SetText(_controlsPanel.Visible ? "Hide Controls" : "ShowControls");
				}
			};


			Append(_controlsPanel);
			Append(_editCameraIcon);
			Append(_editCameraHoverTip);
			Append(_toggleControlsPanelButton);
		}

		public void Dispose() { }

		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			if (!Terraria3D.Enabled) return;
			if (!Main.playerInventory && !InputTerraria3D.CameraControlsEnabled) return;

			if (InputTerraria3D.CameraControlsEnabled)
			{
				_controlsPanel.Draw(spriteBatch);
				_toggleControlsPanelButton.Draw(spriteBatch);
			}
			_editCameraIcon.Draw(spriteBatch);
			if (_editCameraIcon.ContainsPoint(Main.MouseScreen))
				_editCameraHoverTip.Draw(spriteBatch);
		}
	}
}
