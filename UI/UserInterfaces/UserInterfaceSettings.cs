using Terraria.UI;

namespace Terraria3D
{
	public class UserInterfaceSettings : GenericUserInterface
	{
		private UISettingsWindow _settingsWindow;

		public UserInterfaceSettings(string layerName) : base (layerName)
		{
			Visible = false;
			_settingsWindow = new UISettingsWindow("Settings");
			State.Append(_settingsWindow);
		}

		public override void Dispose()
		{
			base.Dispose();
			_settingsWindow?.Dispose();
			_settingsWindow = null;
		}
	}
}