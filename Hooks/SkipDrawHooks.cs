
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Effects;

namespace Terraria3D
{
    public static partial class Hooks
    {
        public static void AddSkip2DHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchLdcI4(1),
                                   i => i.MatchLdcI4(1),
                                   i => i.MatchCallvirt<OverlayManager>("Draw")))
            {
                int startIndex = cursor.Index;
                cursor.Index++;
                cursor.EmitDelegate(() =>
                {
                    if (Main.gameMenu || Main.mapFullscreen) return;
                    Main.graphics.GraphicsDevice.SetRenderTarget(Renderers.ScreenTarget);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);
                });

                if (cursor.TryGotoNext(i => i.MatchCall<PlayerInput>("SetZoom_UI")))
                {
                    if (cursor.TryGotoPrev(i => i.MatchCallvirt<SpriteBatch>("End")))
                    {
                        cursor.Index++;
                        cursor.EmitDelegate(() => Main.graphics.GraphicsDevice.SetRenderTarget(null));
                    }
                }

                if (cursor.TryGotoNext(i => i.MatchLdcI4(37),
                                      i => i.MatchCall("Terraria.TimeLogger", "DetailedDrawTime")))
                {
                    cursor.Index++;
                    cursor.EmitDelegate(() =>
                    {
                        Main.graphics.GraphicsDevice.SetRenderTarget(null);
                        if (Main.hideUI)
                            Terraria3D.Instance.DrawScene();
                    });
                    cursor.Index = startIndex;
                }
            }
        }
    }
}