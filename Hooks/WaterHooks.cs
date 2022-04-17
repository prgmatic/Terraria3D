using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Terraria3D
{
    public static class WaterHooks
    {
        public static void ApplyWaterHook()
        {
            IL.Terraria.Main.DrawTiles += (il) =>
            {
                var loadTextureInstruction = il.Body.Instructions.FirstOrDefault(i => i != null && i.Operand != null && i.Operand.ToString() == "Microsoft.Xna.Framework.Graphics.Texture2D[] Terraria.Main::liquidTexture");
                if (loadTextureInstruction != null)
                {
                    var preDrawWaterCursor = new ILCursor(il);
                    preDrawWaterCursor.Goto(loadTextureInstruction);
                    preDrawWaterCursor.Index--;

                    var preSetupWaterTileCursor = new ILCursor(preDrawWaterCursor);
                    if(preSetupWaterTileCursor.TryGotoPrev(i => i.MatchLdarg(1)))
                    {
                        var jumpToWaterDrawCursor = new ILCursor(il);
                        jumpToWaterDrawCursor.Goto(0);

                        if (jumpToWaterDrawCursor.TryGotoNext(i => i.MatchCallvirt<Tile>("active")) &&
                            jumpToWaterDrawCursor.TryGotoNext(i => i.OpCode == OpCodes.Bne_Un))
                        {
                            jumpToWaterDrawCursor.Index++;
                            jumpToWaterDrawCursor.EmitDelegate<Func<bool>>(() =>
                            {
                                // TODO: Fix Rendering Call
                                
                                // if (Rendering.RenderHalfBlockWaterHack)
                                //     Main.NewText("jump to water!");
                                //return Rendering.RenderHalfBlockWaterHack;
                                
                                return false;
                            });
                            jumpToWaterDrawCursor.Emit(OpCodes.Brtrue_S, preSetupWaterTileCursor.Next);
                        }
                    }

                    var postDrawWaterCursor = new ILCursor(preDrawWaterCursor);
                    if (postDrawWaterCursor.TryGotoNext(i => i.MatchCallvirt<SpriteBatch>("Draw")))
                    {
                        postDrawWaterCursor.Index++;
                        // TODO: Fix rendering call
                        //preDrawWaterCursor.EmitDelegate<Func<bool>>(() => Rendering.RenderHalfBlockWaterHack);
                        preDrawWaterCursor.Emit(OpCodes.Brfalse_S, postDrawWaterCursor.Next);
                    }
                }
            };
        }
    }
}
