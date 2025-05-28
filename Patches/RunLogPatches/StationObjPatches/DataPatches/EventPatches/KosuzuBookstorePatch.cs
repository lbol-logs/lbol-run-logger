using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Shared23;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class KosuzuBookstorePatch
    {
        [HarmonyPatch(typeof(KosuzuBookstore), nameof(KosuzuBookstore.InitVariables)), HarmonyPostfix]
        private static void AddData(KosuzuBookstore __instance)
        {
            if (!Instance.IsInitialized) return;

            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$thirdBook", out bool thirdBook);
            int count = thirdBook ? 3 : 2;
            int[] exhibitsKeys = Enumerable.Range(0, count).ToArray();
            List<string> exhibits = Helpers.GetStorageList<string, int>(storage, exhibitsKeys, "$book");
            Helpers.AddDataValue("Exhibits", exhibits);

            storage.TryGetValue("$returnBookCount", out float returnBookCount);
            int k = (int)returnBookCount;
            if (k == 0) return;

            int[] returnsKeys = Enumerable.Range(0, k).ToArray();
            List<string> returns = Helpers.GetStorageList<string, int>(storage, returnsKeys, "$returnBook");
            Helpers.AddDataValue("Returns", returns);
        }
    }
}