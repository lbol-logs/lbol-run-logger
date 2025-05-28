using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class ShinmyoumaruForgePatch
    {
        [HarmonyPatch(typeof(ShinmyoumaruForge), nameof(ShinmyoumaruForge.InitVariables)), HarmonyPostfix]
        private static void AddData(ShinmyoumaruForge __instance)
        {
            if (!Instance.IsInitialized) return;

            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$hasUpgradableBasics", out bool hasUpgradableBasics);
            storage.TryGetValue("$hasNonBasics", out bool hasNonBasics);
            storage.TryGetValue("$loseMax", out float loseMax);
            Helpers.AddDataValue("HasUpgradableBasics", hasUpgradableBasics);
            Helpers.AddDataValue("HasNonBasics", hasNonBasics);
            Helpers.AddDataValue("LoseMax", (int)loseMax);
        }
    }
}