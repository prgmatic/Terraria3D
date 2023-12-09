using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using Mono.Cecil;
using Terraria.GameContent.Drawing;
using Terraria;

namespace Terraria3D;

public static partial class Hooks
{
    
    // HOOK FUNCTION:
    // This hook swaps the first call to TileDrawing.IsTileDrawLayerSolid for 
    // TileSolidOverrides.IsTileSolid. The original method is a instance method
    // so we must remove .ldarg0 before calling .IsTileSolid

    // SOURCE REFERENCE =====================
    // if (!tile.active() || IsTileDrawLayerSolid(tile.type) != solidLayer)
    //     continue;
    // ======================================
    
    // IL REFERENCE =========================
    // IL_0223: ldloca.s     tile
    // IL_0225: call         instance bool Terraria.Tile::active()
    // IL_022a: brfalse.s    IL_0242
    // IL_022c: ldarg.0      // REMOVED THIS
    // IL_022d: ldloca.s     tile
    // IL_022f: call         instance unsigned int16& Terraria.Tile::get_type()
    // IL_0234: ldind.u2
    // IL_0235: call         instance bool Terraria.GameContent.Drawing.TileDrawing::IsTileDrawLayerSolid(unsigned int16)
    // ====== CALL ABOVE GETS REPLACED =======
    // IL_023a: ldarg.1      // solidLayer
    // IL_023b: ceq
    // IL_023d: ldc.i4.0
    // IL_023e: ceq
    // IL_0240: br.s         IL_0243
    // IL_0242: ldc.i4.1
    // IL_0243: stloc.s      V_22
    // ======================================
    public static void ApplyDrawTileHooks()
    {
        Terraria.GameContent.Drawing.IL_TileDrawing.Draw += (il) =>
        {
            var cursor = new ILCursor(il);
            cursor.Goto(0);

            if (cursor.TryGotoNext(i => i.MatchCall<TileDrawing>("IsTileDrawLayerSolid")))
            {
                cursor.Remove();
                cursor.Emit(OpCodes.Call, GetIsSolidTileMethod());
                if (cursor.TryGotoPrev(i => i.MatchLdarg(0)))
                    cursor.Remove();
            }

            MethodReference GetIsSolidTileMethod()
            {
                var mBase = typeof(TileSolidOverrides).GetMethod("IsTileSolid",
                    BindingFlags.Static | BindingFlags.Public);
                return cursor.Context.Import(mBase);
            }
        };
    }
}