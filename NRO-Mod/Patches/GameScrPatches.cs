using HarmonyLib;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(GameScr))]
    public class GameScrPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameScr.paint))]
        public static void paint(mGraphics g)
        {
            global::Char me = global::Char.myCharz();
            string new_text = "Level boss: ";
            if (me.mobFocus != null) new_text += me.mobFocus.levelBoss.ToString();
            else new_text = "Bạn chưa chọn con quái nào";

            global::mFont.tahoma_7b_blue.drawString(g, new_text, 10, 70, 0);

            
        }
    }
}
