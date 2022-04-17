using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace Terraria3D
{
    public static class TileSolidOverrides
    {
        private static readonly Dictionary<int, bool> _overrides = new Dictionary<int, bool>()
        {
            {TileID.OpenDoor, false }
        };

        public static bool MoveSolidTopToSolid { get; set; } = true;

        public static bool IsTileSolid(Tile tile)
        {
            var result = Main.tileSolid[tile.TileType];
            if (tile.TileType == 11)
                result = true;

            if (MoveSolidTopToSolid)
            {
                if (Main.tileSolidTop[tile.TileType])
                    result = true;
            }

            if (_overrides.ContainsKey(tile.TileType))
                return _overrides[tile.TileType];
            return result;
        }
    }
}