using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage1;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class ParseeJealousyPatch
    {
        [HarmonyPatch(typeof(ParseeJealousy), nameof(ParseeJealousy.InitVariables)), HarmonyPostfix]
        private static void AddExhibit(ParseeJealousy __instance)
        {
            if (!Controller.ShowRandomResult) return;
            __instance.Storage.TryGetValue("$exhibitPassBy", out string exhibit);
            Helpers.AddDataValue("Exhibit", exhibit);
        }

        [HarmonyPatch(typeof(ParseeJealousy), nameof(ParseeJealousy.GetExhibit)), HarmonyPostfix]
        private static void AddExhibits(ParseeJealousy __instance)
        {
            DialogStorage storage = __instance.Storage;
            List<string> exhibits = Helpers.GetStorageList<string, string>(storage, new[] { "", "2" }, "$exhibit");
            Helpers.AddDataValue("Exhibits", exhibits);
        }
    }
}