using BepInEx;
using HarmonyLib;
using System.Reflection;


namespace NRO_Mod.Core
{
    // Bắt buộc phải có Attribute này để BepInEx nhận diện
    [BepInPlugin("com.nro.mod", "NRO Extension Mod", "1.0.0")]
    public class Main : BaseUnityPlugin
    {
        void Awake()
        {
            // Khởi tạo Harmony
            var harmony = new Harmony("com.nro.mod");

            // Quét toàn bộ project để tìm các "Class gương" (Patches)
            // Assembly.GetExecutingAssembly() đảm bảo Harmony tìm đúng các tệp trong DLL của bạn
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.LogInfo("NRO Mod đã được kích hoạt thành công!");
        }
    }
}
