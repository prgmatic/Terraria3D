using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;

namespace Terraria3D;

public static partial class Hooks
{
    public static void ApplyDrawTileHooks()
    {
        IL.Terraria.Main.DrawTiles += (il) =>
        {
            var cursor = new ILCursor(il);
            cursor.Goto(0);

            // Find if(tile.active()...
            if(cursor.TryGotoNext(i => i.MatchCallvirt<Tile>("active")))
            {
                cursor.Index--;
                // Get variable that holds reference to the tile.
                var vTile = cursor.Next.Operand;

                // Get variable 'flag' which determines if tile
                // is solid.
                var vProbeCursor = new ILCursor(cursor);
                if(vProbeCursor.TryGotoPrev(i => i.MatchLdsfld<Main>("tileSolid")))
                {
                    vProbeCursor.Index += 3;
                    var vIsSoled = vProbeCursor.Next.Operand;

                    cursor.Emit(OpCodes.Ldloc_S, vTile);
                    cursor.EmitDelegate<Func<Tile, bool>>((tile) => TileSolidOverrides.IsTileSolid(tile));
                    cursor.Emit(OpCodes.Stloc_S, vIsSoled);
                    cursor.Index -= 5;
                    var newJumpTo = cursor.Next;
                    if (cursor.TryGotoPrev(i => i.OpCode == OpCodes.Bne_Un_S))
                        cursor.Next.Operand = newJumpTo;
                }
            }
        };
    }
}