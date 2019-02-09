using System;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;

namespace Terraria3D
{
	public class UserInterfaceOverlay : GenericUserInterface
	{
		private UIText _cameraStateStatusText;
		public UserInterfaceOverlay(string layerName) : base(layerName)
		{
			_cameraStateStatusText = new UIText(string.Empty);
			_cameraStateStatusText.VAlign = 1;
			_cameraStateStatusText.Left.Pixels = 10;
			_cameraStateStatusText.Top.Pixels = -10;
			State.Append(_cameraStateStatusText);
		}

		public override void Dispose()
		{
			base.Dispose();
			_cameraStateStatusText = null;
		}

		private void OnCameraControlsEnabled(object sender, EventArgs e)
		{
			_cameraStateStatusText.SetText("Camera Edit Mode");
			Visible = true;
		}

		private async void OnCameraControlsDisabled(object sender, EventArgs e)
		{
			_cameraStateStatusText.SetText("Camera Edit Mode Disabled");
			await Task.Delay(TimeSpan.FromSeconds(2));
			Visible = false;
		}
	}
}
