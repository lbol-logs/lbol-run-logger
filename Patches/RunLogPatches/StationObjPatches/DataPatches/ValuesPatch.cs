using HarmonyLib;
using LBoL.Core.Dialogs;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class ValuesPatch
    {
        [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.AdventureRand)), HarmonyPostfix]
        private static void AddValues(int __result)
        {
            if (!Instance.IsInitialized) return;
            Helpers.AddDataListItem("Values", __result);
        }
    }
}