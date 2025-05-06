using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class RinnosukeTradePatch
    {
        [HarmonyPatch(typeof(RinnosukeTrade), nameof(RinnosukeTrade.InitVariables)), HarmonyPostfix]
        private static void AddPrices(RinnosukeTrade __instance)
        {
            DialogStorage storage = __instance.Storage;
            List<int> list = new List<int>();
            for (int i = 1; i <= 2; i++)
            {
                storage.TryGetValue($"$canSell{i}", out bool canSell);
                if (canSell) list.Add(i);
            }
            if (list.Count == 0) return;

            int[] keys = list.ToArray();
            List<string> exhibits = Helpers.GetStorageList<string, int>(storage, keys, "$exhibit");
            List<float> exhibitPrices = Helpers.GetStorageList<float, int>(storage, keys, "$exhibit", "Price");

            Dictionary<string, int> prices = new Dictionary<string, int>();
            for (int i = 0; i < exhibits.Count; i++)
            {
                if (exhibits[i] != null)
                prices.Add(exhibits[i], (int)exhibitPrices[i]);
            }
            Helpers.AddDataValue("Prices", prices);
        }
    }
}