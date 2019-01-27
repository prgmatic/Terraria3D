
using Terraria.GameInput;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.RuntimeDetour.HookGen;

namespace Terraria3D
{
    public static partial class Hooks
    {
        private static void ApplyDrawHooks()
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
                    cursor.Index++;
                    cursor.EmitDelegate(() => Terraria3D.Instance.DrawScene());
                }
            }
        }
    }
}
