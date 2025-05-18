using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class RingoEmpPatch
    {
        [HarmonyPatch(typeof(RingoEmp), nameof(RingoEmp.InitVariables)), HarmonyPostfix]
        private static void AddCards(RingoEmp __instance)
        {
            if (!Controller.ShowRandomResult) return;
            DialogStorage storage = __instance.Storage;
            List<string> cards = Helpers.GetStorageList<string, int>(storage, new[] { 1, 2, 3 }, "$tool");
            Helpers.AddDataValue("Cards", cards);
        }
    }
}