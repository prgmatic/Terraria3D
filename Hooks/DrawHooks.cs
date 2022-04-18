using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace Terraria3D;

public static partial class Hooks
{
    private static void ApplyDrawHooks()
    {
        IL.Terraria.Main.DoDraw += (il) =>
        {
            var cursor = new ILCursor(il);
            cursor.Goto(0);

            // TODO: add pre render hook back
            PreRenderHook(cursor);
            DrawSceneHook(cursor);
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
    // bgParallax = 0.1;
    // <!---- HOOK HERE ------>
    // bgStartX = (int)(0.0 - Math.IEEERemainder((double)screenPosition.X ...
    // ======================================
    private static void PreRenderHook(ILCursor cursor)
    {
        //{IL_14f8: stfld System.Double Terraria.Main::bgParallax}
        // Find TimeLogger.DetailedDrawTime(4) call.
        if (cursor.TryGotoNext(i => i.MatchCallvirt<GraphicsDevice>("Clear")))
        {
            cursor.Index += 1;
            // Render layers to targets
            cursor.EmitDelegate(() =>
            {
                if (Terraria3D.Enabled)
                    Terraria3D.Instance.RenderLayersTargets();
            });
        }
    }

    // HOOK LOCATION:
    // This is a two part hook. Pre draw scene and post draw scene.
    // Pre draw scene: Right after the bg is drawn to the back buffer.
    // Post draw scene: After world space UI is drawn to the screen.

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
    public static void DrawSceneHook(ILCursor cursor)
    {
        // Find Overlays.Scene.Draw(Main.spriteBatch, RenderLayers.Landscape, true) call.
        if (cursor.TryGotoNext(i => i.MatchLdcI4(1),
                i => i.MatchLdcI4(1),
                i => i.MatchCallvirt<OverlayManager>("Draw")))
        {
            // Move to after Scene.Draw
            cursor.Index += 3;
            // Make a new cursor so we keep track of where we currently are
            var cursor2 = new ILCursor(cursor);

            // Find this.DrawInterface(gameTime) call and then the spriteBatch.End()
            // preceding it.
            if (cursor2.TryGotoNext(i => i.MatchCall<Main>("DrawInterface")) &&
                cursor2.TryGotoPrev(i => i.MatchCallvirt<SpriteBatch>("End")))
            {
                // Move to after spriteBatch.End();
                cursor2.Index++;
                // Inject a method that draws the 3D scene.
                cursor2.EmitDelegate(() =>
                {
                    // Render the 3D scene
                    if (Terraria3D.Enabled)
                        Terraria3D.Instance.DrawScene();
                });
                // Move cursor to the starting instruction of our 
                // newly injected code.
                cursor2.Index -= 3;

                // Back at our original cursor, we inject a branch.
                // If we want to skip drawing 2D, we jump the functions
                // that we just created with cursor 2.
                // cursor.EmitDelegate(() =>
                // {
                //     // TODO: Fix end capture 
                //     var result =  Terraria3D.Enabled && !Main.gameMenu && !Main.mapFullscreen;
                //     //if (result && !Main.drawToScreen && Main.netMode != 2 && !Main.gameMenu && !Main.mapFullscreen && Lighting.NotRetro && Filters.Scene.CanCapture())
                //     //Filters.Scene.EndCapture();
                //     return result;
                // });
                // cursor.Emit(OpCodes.Brtrue_S, cursor2.Next);
            }
        }
    }
}