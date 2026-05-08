using HarmonyLib;
using System.Threading;

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
                    }
                }
            }

            return true;
        }
    }
}
