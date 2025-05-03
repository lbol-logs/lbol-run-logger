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
        private static readonly int[] Keys = new[] { 1, 2 };

        [HarmonyPatch(typeof(RinnosukeTrade), nameof(RinnosukeTrade.InitVariables)), HarmonyPostfix]
        private static void AddPrices(Supply __instance)
        {
            DialogStorage storage = __instance.Storage;
            List<string> exhibits = Helpers.GetStorageList<string, int>(storage, RinnosukeTradePatch.Keys, "$exhibit");
            List<float> exhibitPrices = Helpers.GetStorageList<float, int>(storage, RinnosukeTradePatch.Keys, "$exhibit", "Price");

            if (exhibits[0] != null)
            {
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
}