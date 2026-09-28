using HarmonyLib;
using LBoL.EntityLib.Adventures.Shared23;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class NueUfoPatch
    {
        [HarmonyPatch(typeof(NueUfo), nameof(NueUfo.InitVariables)), HarmonyPostfix]
        private static void AddLoseMax(NueUfo __instance)
        {
            if (!Instance.IsInitialized) return;

            __instance.Storage.TryGetValue("$loseMax", out float loseMax);
            Helpers.AddDataValue("LoseMax", (int)loseMax);
        }
    }
}