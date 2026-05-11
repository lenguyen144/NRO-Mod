using HarmonyLib;
using NRO_Mod.Features;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(TileMap))]
    public class TileMapPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(TileMap.loadMap))]
        public static void loadMap(int tileId)
        {
            AutoAttackFeature.reset();
            MarkingFeature.loadData();
        }

    }
}
