using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.UI;

namespace Terraria3D;

public static partial class Hooks
{
    private static void ApplyMouseHook()
    {
        // TODO: Hook should happen after this function is called, not in it
        // Can't hook DoUpdate, guessing because I don't have the v0.16
        // TerrariaHooks.dll
        IL.Terraria.GameContent.PortalHelper.UpdatePortalPoints += (il) =>
        {
            var cursor = new ILCursor(il);
            cursor.Goto(0).EmitDelegate<Action>(() =>
            {
                if (!Terraria3D.Enabled) return;
                Main.mouseX = (int)Cursor3D.MousePos3D.X;
                Main.mouseY = (int)Cursor3D.MousePos3D.Y;
            });
        };

        On.Terraria.UI.LegacyGameInterfaceLayer.DrawSelf += (orig, self) =>
        {
            if (InterfaceRendering.Drawing3D)
            {
                var oldX = Main.mouseX;
                var oldY = Main.mouseY;
                Main.mouseX = (int)Cursor3D.MousePos3D.X;
                Main.mouseY = (int)Cursor3D.MousePos3D.Y;
                var result = orig(self);
                Main.mouseX = oldX;
                Main.mouseY = oldY;
                return result;
            }
            return orig(self);
        };

        On.Terraria.UI.GameInterfaceLayer.Draw += (orig, self) =>
        {
            if (!Terraria3D.Enabled || InterfaceRendering.Drawing3D) return orig(self);
            if (self.Name.Equals("Vanilla: Mouse Over") ||
                self.Name.Equals("Vanilla: Interface Logic 4"))
            {
                var oldX = Main.mouseX;
                var oldY = Main.mouseY;
                Main.mouseX = (int)Cursor3D.MousePos3D.X;
                Main.mouseY = (int)Cursor3D.MousePos3D.Y;
                var result = orig(self);
                Main.mouseX = oldX;
                Main.mouseY = oldY;
                return result;
            }
            else if (self.ScaleType == InterfaceScaleType.Game)
                return !Main.hideUI;
            return orig(self);
        };

        IL.Terraria.Main.DrawMouseOver += (il) =>
        {
            var cursor = new ILCursor(il);
            cursor.Goto(0);

            if(cursor.TryGotoNext(i => i.MatchCall<IngameFancyUI>("MouseOver")))
            {
                cursor.Index++;
                    
                // Modify X component
                cursor.Emit(OpCodes.Ldloca_S, il.Body.Variables[0]);
                cursor.EmitDelegate<Func<int>>(() =>
                {
                    return (int)(Cursor3D.MousePos3D.X + Main.screenPosition.X);
                });
                var x = il.Module.ImportReference(typeof(Rectangle).GetField("X"));
                cursor.Emit(OpCodes.Stfld, x);

                // Modify Y component
                cursor.Emit(OpCodes.Ldloca_S, il.Body.Variables[0]);
                cursor.EmitDelegate<Func<int>>(() =>
                {
                    if (Main.player[Main.myPlayer].gravDir == -1f)
                        return (int)(Main.screenPosition.Y + Main.screenHeight - Cursor3D.MousePos3D.Y);
                    return (int)(Cursor3D.MousePos3D.Y + Main.screenPosition.Y);
                });
                var y = il.Module.ImportReference(typeof(Rectangle).GetField("Y"));
                cursor.Emit(OpCodes.Stfld, y);
            }
        };
    }
}