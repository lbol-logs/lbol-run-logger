using HarmonyLib;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class WatatsukiPurifyPatch
    {
        [HarmonyPatch(typeof(WatatsukiPurify), nameof(WatatsukiPurify.InitVariables)), HarmonyPostfix]
        private static void AddLoseMax(WatatsukiPurify __instance)
        {
            if (!Instance.IsInitialized) return;

            __instance.Storage.TryGetValue("$loseMax", out float loseMax);
            Helpers.AddDataValue("LoseMax", (int)loseMax);
        }
    }
}