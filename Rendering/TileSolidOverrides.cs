using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;

namespace Terraria3D;

public static class TileSolidOverrides
{
    private static readonly Dictionary<int, bool> _overrides = new Dictionary<int, bool>()
    {
        {TileID.OpenDoor, false }
    };

    public static bool MoveSolidTopToSolid { get; set; } = true;

    public static bool IsTileSolid(ushort tileType)
    {
        var result = Main.tileSolid[tileType];

        if (TileID.Sets.DrawTileInSolidLayer[tileType].HasValue)
            result = TileID.Sets.DrawTileInSolidLayer[tileType].Value;
        
        if (MoveSolidTopToSolid)
        {
            if (Main.tileSolidTop[tileType])
                result = true;
        }

        if (_overrides.ContainsKey(tileType))
            return _overrides[tileType];
        return result;
    }
}