using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;
using Terraria.Graphics.Effects;
using Mono.Cecil.Cil;
using System;

namespace Terraria3D
{
    public static partial class Hooks
    {
        private static void ApplyDrawHooks()
        {
            IL.Terraria.Main.do_Draw += (il) =>
            {
                var cursor = il.At(0);

                PreRenderHook(cursor);
                DrawSceneHook(cursor);
                DrawIfUIHiddenHook(cursor);
            };
        }

        // HOOK LOCATION:
        // This hook is inserted right before the background is drawn and nothing
        // has been drawn to the back buffer yet.

        // HOOK FUNCTION:
        // This is where we want to start rending 3D layers to their render targets.

        // SOURCE REFERENCE =====================
        // ...
        //         TimeLogger.DetailedDrawTime(4);
        //     }
        // }
        // this.bgParallax = 0.1;
        // <!---- HOOK HERE ------>
        // this.bgStart = (int)(-Math.IEEERemainder((double)Main.screenPosit ...
        // ======================================
        private static void PreRenderHook(HookILCursor cursor)
        {
            // Find TimeLogger.DetailedDrawTime(4) call.
            if (cursor.TryGotoNext(i => i.MatchLdcI4(4),
                                   i => i.MatchCall("Terraria.TimeLogger", "DetailedDrawTime")))
            {
                cursor.Index += 3;
                // Render layers to targets
                cursor.EmitDelegate(() => Terraria3D.Instance.RenderLayersTargets());
            }
        }

        // HOOK LOCATION:
        // This is a two part hook. Pre draw scene and post draw scene.
        // Pre draw scene: Right after the bg is drawn to the back buffer.
        // Post draw scene: After world space UI is drawn to the sceen.

        // HOOK FUNCTION:
        // We inject a branch into in the pre draw hook that allows us to
        // jump to the post draw hook. This allows us to skip drawing the 
        // scene in 2D. The post draw scene is where we render our scene
        // if UI is visible. We want to draw the 3D scene here so the 
        // it does no occlude the user interface.

        // SOURCE REFERENCE =====================
        // Main.spriteBatch.End();
        // Overlays.Scene.Draw(Main.spriteBatch, RenderLayers.Landscape, true);
        // <!---- PRE DRAW SCENE HOOK HERE ------>
        // Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendS
        // ======================================

        // SOURCE REFERENCE =====================
        // Main.spriteBatch.End();
        // <!---- POST DRAW SCENE HOOK HERE ------>
        // Main.spriteBatch.Begin(SpriteSortM ...
        // PlayerInput.SetZoom_UI();
        // this.DrawFPS();
        // ======================================
        public static void DrawSceneHook(HookILCursor cursor)
        {
            // Find Overlays.Scene.Draw(Main.spriteBatch, RenderLayers.Landscape, true) call.
            if (cursor.TryGotoNext(i => i.MatchLdcI4(1),
                                   i => i.MatchLdcI4(1),
                                   i => i.MatchCallvirt<OverlayManager>("Draw")))
            {
                // Move to after Scene.Draw
                cursor.Index+= 3;
                // Make a new cursor so we keep track of where we currently are
                var cursor2 = new HookILCursor(cursor);

                // Find PlayerInpug.SetZoom_UI() call and then the spriteBatch.End()
                // preceding it.
                if(cursor2.TryGotoNext(i => i.MatchCall<PlayerInput>("SetZoom_UI")) &&
                   cursor2.TryGotoPrev(i => i.MatchCallvirt<SpriteBatch>("End")))
                {
                    // Move to after spriteBatch.End();
                    cursor2.Index++;
                    // Inject a method that draws the 3D scene.
                    cursor2.EmitDelegate(() =>
                    {
                        // Render the 3D scene
                        Terraria3D.Instance.DrawScene();
                    });
                    // Move cursor to the starting instruction of our 
                    // newly injected code.
                    cursor2.Index -= 3;

                    // Back at our original cursor, we inject a branch.
                    // If we want ot skip drawing 2D, we jump the functions
                    // that we just created with cursor 2.
                    cursor.EmitDelegate<Func<bool>>(() =>
                    {
                        var result = !Main.gameMenu && !Main.mapFullscreen;
                        if (result)
                            Filters.Scene.EndCapture();
                        return result;
                    });
                    cursor.Emit(OpCodes.Brtrue_S, cursor2.Next);
                }
            }
        }

        // HOOK LOCATION:
        // After the if (!Main.hideUI) block.

        // HOOK FUNCTION:
        // Draw's the 3D scene if UI is hidden. We normally draw the 3D
        // scene before screen space UI. However, if UI is hidden, our 
        // post scene draw hook will not be called.

        // SOURCE REFERENCE =====================
        // else
        //     Main.maxQ = true;
        // TimeLogger.DetailedDrawTime(37);
        // <!---- HOOK HERE ------>
        //if (Main.mouseLeft)
        // ======================================
        public static void DrawIfUIHiddenHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchLdcI4(37),
                                      i => i.MatchCall("Terraria.TimeLogger", "DetailedDrawTime")))
            {
                cursor.Index++;
                cursor.EmitDelegate(() =>
                {
                    // If UI is hidden, the scene has not been drawn, do it now.
                    if (Main.hideUI)
                        Terraria3D.Instance.DrawScene();
                });
            }
        }
    }
}
