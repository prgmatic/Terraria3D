
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;

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
                StartCapture2DHook(cursor);
                PostInWorldUIRenderHook(cursor);
                DrawIfUIHiddenHook(cursor);
            };
        }

        private static void PreRenderHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchLdcI4(4),
                                   i => i.MatchCall("Terraria.TimeLogger", "DetailedDrawTime")))
            {
                cursor.Index += 3;
                // Render layers to targets
                cursor.EmitDelegate(() => Terraria3D.Instance.RenderLayersTargets());
            }
        }

        public static void StartCapture2DHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchLdcI4(1),
                                   i => i.MatchLdcI4(1),
                                   i => i.MatchCallvirt<OverlayManager>("Draw")))
            {
                int startIndex = cursor.Index;
                cursor.Index++;
                cursor.EmitDelegate(() =>
                {
                    // Don't capture if in main menu or full screen
                    if (Main.gameMenu || Main.mapFullscreen) return;
                    // Start capturing 2D draw calls
                    Main.graphics.GraphicsDevice.SetRenderTarget(Renderers.ScreenTarget);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                });
            }
        }

        private static void PostInWorldUIRenderHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchCall<PlayerInput>("SetZoom_UI")))
            {
                if (cursor.TryGotoPrev(i => i.MatchCallvirt<SpriteBatch>("End")))
                {
                    cursor.Index++;
                    cursor.EmitDelegate(() =>
                    {
                        // Stop capturing draw calls
                        Main.graphics.GraphicsDevice.SetRenderTarget(null);
                        // Render the 3D scene
                        Terraria3D.Instance.DrawScene();
                    });
                }
            }
        }

        public static void DrawIfUIHiddenHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchLdcI4(37),
                                      i => i.MatchCall("Terraria.TimeLogger", "DetailedDrawTime")))
            {
                cursor.Index++;
                cursor.EmitDelegate(() =>
                {
                    // Stop capturing drawing
                    Main.graphics.GraphicsDevice.SetRenderTarget(null);
                    // If UI is hidden, the scene has not been drawn, do it now.
                    if (Main.hideUI)
                        Terraria3D.Instance.DrawScene();
                });
            }
        }
    }
}
