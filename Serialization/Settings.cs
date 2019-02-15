using Newtonsoft.Json;
using Terraria3D.Serialization;
using System.IO;
using Terraria;

namespace Terraria3D
{
	public static class Settings
	{
		public delegate void SettingsLoadedEvent(SettingsData data);
		public static event SettingsLoadedEvent SettingsLoaded;
		public static event SettingsLoadedEvent SettingsSaved;

		public static void Load()
		{
			try
			{
				var json = FileUtils.GetJSON(Terraria3D.Instance.DisplayName);
				var settings = JsonConvert.DeserializeObject<SettingsData>(json);
				settings.Apply(Terraria3D.Instance);
				SettingsLoaded?.Invoke(settings);
			}
			// If not settings file is found, no sweat. Don't do anything.
			catch(FileNotFoundException) { }
			catch(DirectoryNotFoundException) { }
			catch
			{
				Terraria3D.Instance.Scene.CameraDriver.ResetCameraPosition();
				Main.NewText(string.Format("{0}: Could not load settings from last session.", Terraria3D.Instance.DisplayName));
			}
		}

		public static void Save()
		{
			var settings = new SettingsData();
			settings.Record(Terraria3D.Instance);
			var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
			FileUtils.SaveJSON(json, Terraria3D.Instance.DisplayName);
			SettingsSaved?.Invoke(settings);
		}
	}
}
