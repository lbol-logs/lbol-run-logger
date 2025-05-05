using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Shared12;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class KeineSalesPatch
    {
        [HarmonyPatch(typeof(KeineSales), nameof(KeineSales.InitVariables)), HarmonyPostfix]
        private static void AddQuestions(KeineSales __instance)
        {
            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$stageNo", out float stageNo);
            int[] keys = Enumerable.Range(1, (int)stageNo).ToArray();
            List<int> questions = Helpers.GetStorageList<float, int>(storage, keys, "$question", "No").Select(question => (int)question).ToList();
            Helpers.AddDataValue("Questions", questions);
        }
    }
}