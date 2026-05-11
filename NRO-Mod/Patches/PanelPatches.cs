using HarmonyLib;
using NRO_Mod.Features;
using NRO_Mod.Models;
using System;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(Panel))]
    public class PanelPatches
    {
        public static string[] vStrSettingModel;
        public static bool isModPanel = false;
        public static void setTabMod(Panel __instance)
        {
            load_vStrSettingModel();
            Panel.strCauhinh = vStrSettingModel;

            __instance.currentListLength = Panel.strCauhinh.Length;
            __instance.ITEM_HEIGHT = 24;
            __instance.selected = ((!GameCanvas.isTouch) ? 0 : -1);
            __instance.cmyLim = __instance.currentListLength * __instance.ITEM_HEIGHT - __instance.hScroll;
            if (__instance.cmyLim < 0)
            {
                __instance.cmyLim = 0;
            }
            __instance.cmy = (__instance.cmtoY = __instance.cmyLast[__instance.currentTabIndex]);
            if (__instance.cmy < 0)
            {
                __instance.cmy = (__instance.cmtoY = 0);
            }
            if (__instance.cmy > __instance.cmyLim)
            {
                __instance.cmy = (__instance.cmtoY = __instance.cmyLim);
            }
        }

        public static void load_vStrSettingModel()
        {
            vStrSettingModel = new string[2];

            vStrSettingModel[0] = (SettingsModel.IsAutoAttack ? "[X] " : "[ ] ") +  "Auto Attack";
            vStrSettingModel[1] = (SettingsModel.isAttackMarkedMob ? "[X] " : "[ ] ") + "Attack Marked Mob";
        }

        [HarmonyPrefix]
        [HarmonyPatch("setTypeOption")]
        public static bool setTypeOption(Panel __instance)
        {
            if (PanelPatches.isModPanel)
            {
                __instance.type = 19;
                Traverse.Create(__instance).Method("setType", 0).GetValue();
                PanelPatches.setTabMod(__instance);
                __instance.cmx = (__instance.cmtoX = 0);

                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch("doFireTool")]
        public static bool doFireTool(Panel __instance)
        {
            if (!global::Char.myCharz().havePet)
            {
                if (__instance.selected != 7) return true;
            }
            else
            {
                if (__instance.selected != 8) return true;
            }

            if (SoundMn.IsDelAcc && __instance.selected == Panel.strTool.Length - 1)
            {
                Service.gI().sendDelAcc();
                return false;
            }

            PanelPatches.isModPanel = false;
            __instance.setTypeOption();

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("doFireOption")]
        public static bool doFireOption(Panel __instance)
        {
            if (!PanelPatches.isModPanel) return true;

            if (__instance.selected < 0)
            {
                return false;
            }

            switch (__instance.selected)
            {
                case 0:
                    SettingsModel.IsAutoAttack = !SettingsModel.IsAutoAttack;
                    load_vStrSettingModel();
                    Panel.strCauhinh = vStrSettingModel;
                    break;
                case 1:
                    SettingsModel.isAttackMarkedMob = !SettingsModel.isAttackMarkedMob;
                    load_vStrSettingModel();
                    Panel.strCauhinh = vStrSettingModel;
                    break;
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch("paintTab")]
        public static bool paintTab(Panel __instance, mGraphics g)
        {
            if (__instance.type == 19 && PanelPatches.isModPanel)
            {
                g.setColor(13524492);
                g.fillRect(__instance.X + 1, 78, __instance.W - 2, 1);
                mFont.tahoma_7b_dark.drawString(g, "Mod", __instance.xScroll + __instance.wScroll / 2, 59, mFont.CENTER);
                return false;
            }

            return true;
        }

    }
}
