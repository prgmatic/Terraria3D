using Terraria;
using Terraria.ModLoader;

namespace Terraria3D
{
	public class InputTerraria3D
	{
		public static bool CameraControlsEnabled { get; set; }

		private static ModKeybind _settingsKeyBinding;
		private static ModKeybind _toggleCameraControlsKeyBinding;
		private static ModKeybind _toggle3DKeyBinding;
		private static ModKeybind _toggleAOBinding;

		public static void Load()
		{
			// Why 'L'? No clue, I hope it's not already bound to something xD
			_settingsKeyBinding = KeybindLoader.RegisterKeybind(Terraria3D.Instance, "Toggle 3D Settings", "L");
			_toggleCameraControlsKeyBinding = KeybindLoader.RegisterKeybind(Terraria3D.Instance, "Toggle Camera Controls", "C");
			_toggle3DKeyBinding = KeybindLoader.RegisterKeybind(Terraria3D.Instance, "Toggle 3D", "K");
			if (Renderers.SM3Enabled)
				_toggleAOBinding = KeybindLoader.RegisterKeybind(Terraria3D.Instance, "Toggle AO", "None");
		}

		public static void Unload()
		{
			_settingsKeyBinding = null;
			_toggleCameraControlsKeyBinding = null;
			_toggle3DKeyBinding = null;
			_toggleAOBinding = null;
		}

		public static void Update()
		{
			Cursor3D.UpdateMousePos3D();
			// Hack to fix scroll bug.
			//PlayerInput.ScrollWheelDeltaForUI = 0;
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
				Terraria3D.Instance.Toggle();
			if (_settingsKeyBinding.JustPressed)
				UITerraria3D.SettingsInterface.Visible = !UITerraria3D.SettingsInterface.Visible;
			if (!Terraria3D.Enabled) return;
			if (_toggleCameraControlsKeyBinding.JustPressed)
				CameraControlsEnabled = !CameraControlsEnabled;
			if (Renderers.SM3Enabled)
			{
				if (_toggleAOBinding.JustPressed)
					Terraria3D.Instance.Scene.AmbientOcclusion = !Terraria3D.Instance.Scene.AmbientOcclusion;
			}
		}
	}
}