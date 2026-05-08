using HarmonyLib;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(Char))]
    public class CharPatches
    {
        [HarmonyPatch(nameof(Char.update))]
        [HarmonyPostfix]
        public static void update(Char __instance)
        {
            if (__instance.me && Models.SettingsModel.IsAutoAttack)
            {
                Features.AutoAttackFeature.execute();
            }
        }
    }
}
