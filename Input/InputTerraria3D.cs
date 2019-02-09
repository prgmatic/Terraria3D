using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Terraria3D
{
	public class InputTerraria3D
	{
		public static bool CameraControlsEnabled { get; private set; } = false;

		private static ModHotKey _settingsKeyBinding;
		private static ModHotKey _toggleCameraControlsKeyBinding;
		private static ModHotKey _toggle3DKeyBinding;
		private static ModHotKey _toggleAOBinding;

		public static void Load()
		{
			// Why 'L'? No clue, I hope it's not already bound to something xD
			_settingsKeyBinding = Terraria3D.Instance.RegisterHotKey("Toggle 3D Settings", "L");
			_toggleCameraControlsKeyBinding = Terraria3D.Instance.RegisterHotKey("Toggle Camera Controls", "Multiply");
			_toggle3DKeyBinding = Terraria3D.Instance.RegisterHotKey("Toggle 3D", "K");
			_toggleAOBinding = Terraria3D.Instance.RegisterHotKey("Toggle AO", "None");
		}

		public static void Unload()
		{
			_settingsKeyBinding = null;
			_toggleCameraControlsKeyBinding = null;
			_toggle3DKeyBinding = null;
			_toggleAOBinding = null;
		}

		public static void Update(GameTime gameTime)
		{
			Cursor3D.UpdateMousePos3D();
			// Hack to fix scroll bug.
			PlayerInput.ScrollWheelDeltaForUI = 0;
		}

		public static void SetControls(Player player)
		{
			// If driving the camera, consume player movement 
			// events.
			if (Terraria3D.Enabled && CameraControlsEnabled)
			{
				player.controlLeft = false;
				player.controlRight = false;
				player.controlUp = false;
				player.controlDown = false;

				player.controlUseItem = false;
				player.controlThrow = false;

				player.controlHook = false;
				player.controlQuickHeal = false;
				player.controlQuickMana = false;

				player.mouseInterface = true;
				player.lastMouseInterface = true;
			}
		}

		public static void ProcessInput()
		{
			if (_toggle3DKeyBinding.JustPressed)
				Terraria3D.Enabled = !Terraria3D.Enabled;
			if (_settingsKeyBinding.JustPressed)
				UITerraria3D.SettingsInterface.Visible = !UITerraria3D.SettingsInterface.Visible;
			if (!Terraria3D.Enabled) return;
			if (_toggleCameraControlsKeyBinding.JustPressed)
				CameraControlsEnabled = !CameraControlsEnabled;
			if (_toggleAOBinding.JustPressed)
				Terraria3D.Instance.Scene.AmbientOcclusion = !Terraria3D.Instance.Scene.AmbientOcclusion;
		}
	}
}