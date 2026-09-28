using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Shared23;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class NarekoPuzzlePatch
    {
        [HarmonyPatch(typeof(NarekoPuzzle), nameof(NarekoPuzzle.InitVariables)), HarmonyPostfix]
        private static void AddData(NarekoPuzzle __instance)
        {
            if (!Instance.IsInitialized) return;

            DialogStorage storage = __instance.Storage;
            List<string> cards = Helpers.GetStorageList<string, string>(storage, new[] { "A", "B", "C" }, "$card");
            List<bool> isUpgradeds = Helpers.GetStorageList<bool, string>(storage, new[] { "A", "B", "C" }, "$isUpgraded");
            List<string> exhibits = Helpers.GetStorageList<string, string>(storage, new[] { "B", "C" }, "$exhibit");
            Dictionary<string, object> trades = new Dictionary<string, object>();
            (string, int, int)[] tradeConfigs = new (string, int, int)[]
            {
                ("A", 0, -1),
                ("B", 1,  0),
                ("C", 2,  1)
            };
            foreach ((string key, int c, int e) in tradeConfigs)
            {
                if (string.IsNullOrEmpty(cards[c])) continue;

                Dictionary<string, object> tradeItem = new Dictionary<string, object>()
                {
                    { "Card", cards[c] },
                    { "IsUpgraded", isUpgradeds[c] }
                };
                if (e != -1) tradeItem["Exhibit"] = exhibits[e];
                trades[key] = tradeItem;
            }
            Helpers.AddDataValue("Trade", trades);
        }
    }
}