using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;
using System.Diagnostics;
using Terraria.GameInput;

namespace Terraria3D
{
    public static class Hooks
    {
        public static void Initialize()
        {
            IL.Terraria.Main.do_Draw += (il) =>
            {
                var cursor = il.At(0);

                AddPreRenderHook(cursor);
                AddPostSceneRenderHook(cursor);
            };

        }

        private static void AddPreRenderHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchLdcI4(4),
                                   i => i.MatchCall("Terraria.TimeLogger", "DetailedDrawTime")))
            {
                cursor.Index += 3;
                cursor.EmitDelegate(() => Terraria3D.Instance.RenderLayersTargets());
            }
        }

        private static void AddPostSceneRenderHook(HookILCursor cursor)
        {
            if (cursor.TryGotoNext(i => i.MatchCall<PlayerInput>("SetZoom_UI")))
            {
                if (cursor.TryGotoPrev(i => i.MatchCallvirt<SpriteBatch>("End")))
                {
                    Trace.WriteLine(cursor.Next.Offset);
                    cursor.Index++;
                    cursor.EmitDelegate(() => Terraria3D.Instance.DrawScene());
                }
            }
        }
    }
}
