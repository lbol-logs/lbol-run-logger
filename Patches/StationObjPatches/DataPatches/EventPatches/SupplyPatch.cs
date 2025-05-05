using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class SupplyPatch
    {
        [HarmonyPatch(typeof(Supply), nameof(Supply.InitVariables)), HarmonyPostfix]
        private static void AddData(Supply __instance)
        {
            DialogStorage storage = __instance.Storage;
            List<string> exhibits = Helpers.GetStorageList<string, string>(storage, new[] { "A", "B" }, "$exhibit");
            Helpers.AddDataValue("Exhibits", exhibits);

            storage.TryGetValue("$bothFlag", out bool both);
            Helpers.AddDataValue("Both", both);
        }
    }
}