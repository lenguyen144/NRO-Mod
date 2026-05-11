using HarmonyLib;
using NRO_Mod.Features;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(Mob))]
    public class MobPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Mob.paint))]
        public static bool paint(Mob __instance, mGraphics g)
        {
            highlight_VMarkedMob(g);

            return true;
        }

        public static void highlight_VMarkedMob(mGraphics g)
        {
            for (int i = 0; i < MarkingFeature.vMarkedMob.size(); i++)
            {
                Mob mob = (Mob)MarkingFeature.vMarkedMob.elementAt(i);
                global::mFont.tahoma_7b_red.drawString(g, "X", mob.x, mob.y - mob.h - 20, mFont.CENTER);
            }
        }

    }
}
