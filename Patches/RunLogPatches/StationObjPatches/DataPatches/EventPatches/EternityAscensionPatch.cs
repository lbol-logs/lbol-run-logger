using HarmonyLib;
using LBoL.EntityLib.Adventures.Stage1;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class EternityAscensionPatch
    {
        [HarmonyPatch(typeof(EternityAscension), nameof(EternityAscension.InitVariables)), HarmonyPostfix]
        private static void AddCard(EternityAscension __instance)
        {
            if (!Instance.IsInitialized) return;

            if (!Controller.ShowRandomResult) return;
            __instance.Storage.TryGetValue("$transformCard", out string card);
            Helpers.AddDataValue("Card", card);
        }
    }
}