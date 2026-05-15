using HarmonyLib;
using NRO_Mod.Features;
using NRO_Mod.Models;
using System;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(GameScr))]
    public class GameScrPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameScr.paint))]
        public static void paint(mGraphics g)
        {
            global::mFont.tahoma_7_white.drawString(g, Models.SettingsModel.IsAutoAttack ? "Auto Attack: bật" : "Auto Attack: tắt", 10, 70, 0);
            
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameScr.updateKey))]
        public static bool updateKey(GameScr __instance)
        {
            if (GameCanvas.keyAsciiPress != 0)
            {
                if (__instance.mobCapcha == null)
                {
                    if (TField.isQwerty)
                    {
                        if (GameCanvas.keyAsciiPress == 97)
                        {
                            Models.SettingsModel.IsAutoAttack = !Models.SettingsModel.IsAutoAttack;
                            string text = Models.SettingsModel.IsAutoAttack ? "Bạn đã bật tự động đánh" : "Bạn đã tắt tự động đánh";

                            GameScr.info1.addInfo(text, 0);
                        }
                        else if (GameCanvas.keyAsciiPress == 109)
                        {
                            global::Char me = global::Char.myCharz();
                            if (me != null)
                            {
                                if (me.mobFocus != null)
                                {
                                    MarkingFeature.toggleMarkedMob(me.mobFocus.mobId, TileMap.mapID);
                                }
                                else
                                {
                                    GameScr.info1.addInfo("Bạn chưa chọn mục tiêu nào", 0);
                                }
                            }
                        }
                        else if (GameCanvas.keyAsciiPress == 108)
                        {
                            GameCanvas.keyAsciiPress = 0;
                            PanelPatches.setTypeMod(GameCanvas.panel);
                            GameCanvas.panel.show();
                        }
                    }
                }
            }

            return true;
        }
    }
}
