using HarmonyLib;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(Mob))]
    public class MobPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Mob.paint))]
        public static bool paint(Mob __instance, mGraphics g)
        {
            global::Char me = global::Char.myCharz();
            if (me.mobFocus != null && me.mobFocus == __instance)
            {
                global::mFont.tahoma_7b_white.drawString(g, "Mày xui rồi con", me.mobFocus.x, me.mobFocus.y - me.mobFocus.h - 20, mFont.CENTER);
            }

            return true;
        }

    }
}
