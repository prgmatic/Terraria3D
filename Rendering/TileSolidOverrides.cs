using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Terraria3D
{
    public static class TileSolidOverrides
    {
        private static Dictionary<int, bool> _overrides = new Dictionary<int, bool>()
        {
            {TileID.OpenDoor, false }
        };

        public static bool MoveSolidTopToSolid { get; set; } = true;

        public static bool IsTileSolid(Tile tile)
        {
            var result = Main.tileSolid[tile.type];
            if (tile.type == 11)
                result = true;

            if (MoveSolidTopToSolid)
            {
                if (Main.tileSolidTop[tile.type])
                    result = true;
            }

            if (_overrides.ContainsKey(tile.type))
                return _overrides[tile.type];
            return result;
        }
    }
}