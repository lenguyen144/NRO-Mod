using HarmonyLib;
using NRO_Mod.Models;
using System;

namespace NRO_Mod.Patches
{
    [HarmonyPatch(typeof(Panel))]
    public class PanelPatches
    {
        public static string[] vStrSettingModel;
        public static int length_mainOfTagName;

        private static readonly FastInvokeHandler IsTabInven_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "isTabInven"));
        private static readonly FastInvokeHandler updateCombine_EffInvoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "updateCombineEff"));
        private static readonly FastInvokeHandler updateKeyInTabBar_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "updateKeyInTabBar"));
        private static readonly FastInvokeHandler paintTopInfo_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "paintTopInfo"));
        private static readonly FastInvokeHandler updateKeyScrollView_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "updateKeyScrollView"));
        private static readonly FastInvokeHandler paintBottomMoneyInfo_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "paintBottomMoneyInfo"));
        private static readonly FastInvokeHandler paintTab_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "paintTab"));
        private static readonly FastInvokeHandler paintToolInfo_Invoker = MethodInvoker.GetHandler(AccessTools.Method(typeof(Panel), "paintToolInfo"));


        [HarmonyPrefix]
        [HarmonyPatch("init")]
        public static bool init(Panel __instance, ref int ___pX, ref int ___pY)
        {
            ___pX = GameCanvas.pxLast + __instance.cmxMap;
            ___pY = GameCanvas.pyLast + __instance.cmyMap;

            string[][][] New_tabName = new string[__instance.tabName.Length + 1][][];
            Array.Copy(__instance.tabName, New_tabName, __instance.tabName.Length);
            PanelPatches.length_mainOfTagName = __instance.tabName.Length;
            New_tabName[PanelPatches.length_mainOfTagName] = new string[][]
            {
                new string[]
                {
                    "Mod 1"
                },
                new string[]
                {
                    "Mod 2" 
                }
            };

            __instance.tabName = New_tabName;

            __instance.lastTabIndex = new int[__instance.tabName.Length];
            for (int i = 0; i < __instance.lastTabIndex.Length; i++)
            {
                __instance.lastTabIndex[i] = -1;
            }

            return false;
        }


        [HarmonyPrefix]
        [HarmonyPatch("paint")]
        public static bool paint(Panel __instance, mGraphics g)
        {
            if (__instance.type < PanelPatches.length_mainOfTagName) return true;

            g.translate(-g.getTranslateX(), -g.getTranslateY() + mGraphics.addYWhenOpenKeyBoard);
            g.translate(-__instance.cmx, 0);
            g.translate(__instance.X, __instance.Y);
            if ((int)GameCanvas.panel.combineSuccess != -1)
            {
                if (__instance.Equals(GameCanvas.panel))
                {
                    __instance.paintCombineEff(g);
                }
            }
            else
            {
                GameCanvas.paintz.paintFrameSimple(__instance.X, __instance.Y, __instance.W, __instance.H, g);
                try
                {
                    paintTopInfo_Invoker(__instance, new object[] { g });
                }
                catch (Exception ex)
                {
                }

                paintBottomMoneyInfo_Invoker(__instance, new object[] { g });
                paintTab_Invoker(__instance, new object[] { g });

                if (__instance.type >= PanelPatches.length_mainOfTagName)
                {
                    PanelPatches.paintMod(__instance, g);
                }
                GameScr.resetTranslate(g);
                __instance.paintDetail(g);
                if (__instance.cmx == __instance.cmtoX && !GameCanvas.menu.showMenu)
                {
                    __instance.cmdClose.paint(g);
                }
                if (__instance.tabIcon != null && __instance.tabIcon.isShow)
                {
                    __instance.tabIcon.paint(g);
                }
                g.translate(-g.getTranslateX(), -g.getTranslateY());
                g.translate(__instance.X, __instance.Y);
                g.translate(-__instance.cmx, 0);
            }

            return false;
        }


        [HarmonyPrefix]
        [HarmonyPatch("update")]
        public static bool update(Panel __instance, ref int ___delayKigui, ref bool ___isKiguiXu, ref bool ___isKiguiLuong,
                                  Scroll ___scroll, bool ___isnewInventory, ref Effect ___eBanner, ref int ___waitToPerform,
                                  ref int[] ___lastSelect)
        {
            if (__instance.type < PanelPatches.length_mainOfTagName) return true;

            if (__instance.chatTField != null && __instance.chatTField.isShow)
            {
                __instance.chatTField.update();
                return false;
            }
            if (___isKiguiXu)
            {
                ___delayKigui++;
                if (___delayKigui == 10)
                {
                    ___delayKigui = 0;
                    ___isKiguiXu = false;
                    __instance.chatTField.tfChat.setText(string.Empty);
                    __instance.chatTField.strChat = mResources.kiguiXuchat + " ";
                    __instance.chatTField.tfChat.name = mResources.input_money;
                    __instance.chatTField.to = string.Empty;
                    __instance.chatTField.isShow = true;
                    __instance.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    __instance.chatTField.tfChat.setMaxTextLenght(10);
                    if (GameCanvas.isTouch)
                    {
                        __instance.chatTField.tfChat.doChangeToTextBox();
                    }
                    if (Main.isWindowsPhone)
                    {
                        __instance.chatTField.tfChat.strInfo = __instance.chatTField.strChat;
                    }
                    if (!Main.isPC)
                    {
                        __instance.chatTField.startChat2(__instance, string.Empty);
                    }
                }
                return false;
            }
            if (___isKiguiLuong)
            {
                ___delayKigui++;
                if (___delayKigui == 10)
                {
                    ___delayKigui = 0;
                    ___isKiguiLuong = false;
                    __instance.chatTField.tfChat.setText(string.Empty);
                    __instance.chatTField.strChat = mResources.kiguiLuongchat + "  ";
                    __instance.chatTField.tfChat.name = mResources.input_money;
                    __instance.chatTField.to = string.Empty;
                    __instance.chatTField.isShow = true;
                    __instance.chatTField.tfChat.setIputType(TField.INPUT_TYPE_NUMERIC);
                    __instance.chatTField.tfChat.setMaxTextLenght(10);
                    if (GameCanvas.isTouch)
                    {
                        __instance.chatTField.tfChat.doChangeToTextBox();
                    }
                    if (Main.isWindowsPhone)
                    {
                        __instance.chatTField.tfChat.strInfo = __instance.chatTField.strChat;
                    }
                    if (!Main.isPC)
                    {
                        __instance.chatTField.startChat2(__instance, string.Empty);
                    }
                }
                return false;
            }
            if (___scroll != null)
            {
                ___scroll.updatecm();
            }
            if (__instance.tabIcon != null && __instance.tabIcon.isShow)
            {
                __instance.tabIcon.update();
                return false;
            }
            __instance.moveCamera();

            if ((bool)IsTabInven_Invoker(__instance, null) && ___isnewInventory)
            {
                if (___eBanner == null)
                {
                    ___eBanner = new Effect(205, 0, 0, 3, 10, -1);
                    ___eBanner.typeEff = 2;
                }
                if (___eBanner != null)
                {
                    ___eBanner.update();
                }
            }
            if (___waitToPerform > 0)
            {
                ___waitToPerform--;
                if (___waitToPerform == 0)
                {
                    ___lastSelect[__instance.currentTabIndex] = __instance.selected;
                    if (__instance.type == PanelPatches.length_mainOfTagName)
                    {
                        PanelPatches.doFireMod(__instance);
                    }
                }
            }
            for (int i = 0; i < ClanMessage.vMessage.size(); i++)
            {
                ((ClanMessage)ClanMessage.vMessage.elementAt(i)).update();
            }

            updateCombine_EffInvoker(__instance, null);

            return false;

        }


        [HarmonyPrefix]
        [HarmonyPatch("updateKey")]
        public static bool updateKey(Panel __instance, ref int ___waitToPerform, ref Command ___left, ref bool ___pointerIsDowning, ref bool ___isClanOption)
        {
            if (__instance.type < PanelPatches.length_mainOfTagName) return true;

            if (__instance.chatTField != null && __instance.chatTField.isShow)
            {
                return false;
            }
            if (!GameCanvas.panel.isDoneCombine)
            {
                return false;
            }
            if (InfoDlg.isShow)
            {
                return false;
            }
            if (__instance.tabIcon != null && __instance.tabIcon.isShow)
            {
                __instance.tabIcon.updateKey();
                return false;
            }
            if (__instance.isClose)
            {
                return false;
            }
            if (!__instance.isShow)
            {
                return false;
            }
            if (__instance.cmdClose.isPointerPressInside())
            {
                __instance.cmdClose.performAction();
                return false;
            }
            if (GameCanvas.keyPressed[13])
            {
                if (__instance.type != 4)
                {
                    __instance.hide();
                    return false;
                }
                __instance.setTypeMain();
                __instance.cmx = (__instance.cmtoX = 0);
            }
            if (GameCanvas.keyPressed[12] || GameCanvas.keyPressed[(!Main.isPC) ? 5 : 25])
            {
                if (___left.idAction > 0)
                {
                    __instance.perform(___left.idAction, ___left.p);
                }
                else
                {
                    ___waitToPerform = 2;
                }
            }
            if (__instance.Equals(GameCanvas.panel) && GameCanvas.panel2 == null && GameCanvas.isPointerJustRelease && !GameCanvas.isPointer(__instance.X, __instance.Y, __instance.W, __instance.H) && !___pointerIsDowning)
            {
                __instance.hide();
                return false;
            }
            if (!___isClanOption)
            {
                updateKeyInTabBar_Invoker(__instance, null);
            }
            if (__instance.type == PanelPatches.length_mainOfTagName)
            {
                PanelPatches.updateKeyMod(__instance);
            }
            GameCanvas.clearKeyHold();
            for (int i = 0; i < GameCanvas.keyPressed.Length; i++)
            {
                GameCanvas.keyPressed[i] = false;
            }

            return false;

        }


        [HarmonyPrefix]
        [HarmonyPatch("paintTopInfo")]
        public static bool paintTopInfo(Panel __instance, mGraphics g)
        {
            if (__instance.type < PanelPatches.length_mainOfTagName) return true;

            g.setClip(__instance.X + 1, __instance.Y, __instance.W - 2, __instance.yScroll - 2);
            g.setColor(9993045);
            g.fillRect(__instance.X, __instance.Y, __instance.W - 2, 50);

            if (__instance.type == PanelPatches.length_mainOfTagName)
            {
                SmallImage.drawSmallImage(g, global::Char.myCharz().avatarz(), __instance.X + 25, 50, 0, 33);
                paintToolInfo_Invoker(__instance, new object[] { g});
            }

            return false;
        }

        public static void load_vStrSettingModel()
        {
            vStrSettingModel = new string[2];

            vStrSettingModel[0] = (SettingsModel.IsAutoAttack ? "[X] " : "[ ] ") +  "Auto Attack";
            vStrSettingModel[1] = (SettingsModel.isAttackMarkedMob ? "[X] " : "[ ] ") + "Attack Marked Mob";
        }


        [HarmonyPrefix]
        [HarmonyPatch("paintTab")]
        public static bool paintTab(Panel __instance, mGraphics g)
        {
            if (__instance.type == PanelPatches.length_mainOfTagName)
            {
                g.setColor(13524492);
                g.fillRect(__instance.X + 1, 78, __instance.W - 2, 1);
                mFont.tahoma_7b_dark.drawString(g, "Mod", __instance.xScroll + __instance.wScroll / 2, 59, mFont.CENTER);
                return false;
            }

            return true;
        }

        public static void setTypeMod(Panel __instance)
        {
            __instance.type = PanelPatches.length_mainOfTagName;
            Traverse.Create(__instance).Method("setType", 0).GetValue();
            PanelPatches.setTabMod(__instance);
            __instance.cmx = (__instance.cmtoX = 0);
        }

        private static void setTabMod(Panel __instance)
        {
            PanelPatches.load_vStrSettingModel();
            __instance.currentListLength = PanelPatches.vStrSettingModel.Length;
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

        private static void paintMod(Panel __instance, mGraphics g)
        {
            g.setClip(__instance.xScroll, __instance.yScroll, __instance.wScroll, __instance.hScroll);
            g.translate(0, -__instance.cmy);
            for (int i = 0; i < PanelPatches.vStrSettingModel.Length; i++)
            {
                int x = __instance.xScroll;
                int num = __instance.yScroll + i * __instance.ITEM_HEIGHT;
                int num2 = __instance.wScroll - 1;
                int h = __instance.ITEM_HEIGHT - 1;
                if (num - __instance.cmy <= __instance.yScroll + __instance.hScroll)
                {
                    if (num - __instance.cmy >= __instance.yScroll - __instance.ITEM_HEIGHT)
                    {
                        g.setColor((i != __instance.selected) ? 15196114 : 16383818);
                        g.fillRect(x, num, num2, h);
                        mFont.tahoma_7b_dark.drawString(g, PanelPatches.vStrSettingModel[i], __instance.xScroll + 25, num + 6, mFont.LEFT);
                    }
                }
            }
            Traverse.Create(__instance).Method("paintScrollArrow", g).GetValue();
        }

        private static void doFireMod(Panel __instance)
        {
            if (__instance.selected < 0)
            {
                return;
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
        }

        private static void updateKeyMod(Panel __instance)
        {
            updateKeyScrollView_Invoker(__instance, null);

        }

        

    }
}
