using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Terraria3D
{
	public static class InputUtil
	{
		// Used to check if binding is pressed when input is blocked.
		public static bool KeyBindingDown(ModHotKey hotKey)
		{
			return hotKey.GetAssignedKeys().Any(binding =>
			{
				Keys key;
				if(Enum.TryParse<Keys>(binding, out key))
					return Main.keyState.IsKeyDown(key);
				return false;
			});
		}
	}
}
