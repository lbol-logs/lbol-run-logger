using HarmonyLib;
using RunLogger.Utils;
using LBoL.Presentation;

namespace RunLogger.Patches.SaveData
{
    [HarmonyPatch]
    internal static class SelectProfile
    {
        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SelectProfile)), HarmonyPostfix]
        private static void DestroyInstance()
        {
            BepinexPlugin.log.LogDebug($"Profile #{Helpers.CurrentSaveIndex} selected");
            Controller.DestroyInstance();
        }
    }
}