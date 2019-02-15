using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;

namespace Terraria3D.Serialization
{
	public static class FileUtils
	{
		public static string GetJSON(string relativePath)
		{
			var path = GetPath(relativePath);
			if (!File.Exists(path))
				throw new FileNotFoundException(string.Empty, path);
			return File.ReadAllText(path);
		}

		public static void SaveJSON(string json, string relativePath)
		{
			var path = GetPath(relativePath);
			var directory = Path.GetDirectoryName(path);
			if (!Directory.Exists(directory))
				throw new DirectoryNotFoundException(directory);
			File.WriteAllText(path, json);
		}

		private static string GetPath(string relativePath)
			=> Path.Combine(Main.SavePath, relativePath + ".json");
	}
}
