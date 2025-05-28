using HarmonyLib;
using LBoL.EntityLib.Adventures.Shared23;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class HinaCollectPatch
    {
        [HarmonyPatch(typeof(HinaCollect), nameof(HinaCollect.InitVariables)), HarmonyPostfix]
        private static void AddCard(HinaCollect __instance)
        {
            if (!Instance.IsInitialized) return;

            if (!Controller.ShowRandomResult) return;
            __instance.Storage.TryGetValue("$card", out string card);
            Helpers.AddDataValue("Card", card);
        }
    }
}