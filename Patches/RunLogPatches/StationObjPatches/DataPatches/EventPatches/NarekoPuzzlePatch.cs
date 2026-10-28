using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Shared23;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

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
            (string, bool)[] tradeConfigs = new (string, bool)[]
            {
                ("A", false),
                ("B", true),
                ("C", true)
            };
            List<bool> hasCards = Helpers.GetStorageList<bool, string>(storage, tradeConfigs.Select(x => x.Item1).ToArray(), "$hasCard");

            Dictionary<string, object> trades = new Dictionary<string, object>();
            for (int i = 0; i < tradeConfigs.Length; i++)
            {
                if (!hasCards[i]) continue;

                (string key, bool hasExhibit) = tradeConfigs[i];
                storage.TryGetValue($"$card{key}", out string card);
                storage.TryGetValue($"$isUpgraded{key}", out bool isUpgraded);
                Dictionary<string, object> tradeItem = new Dictionary<string, object>()
                {
                    { "Card", card },
                    { "IsUpgraded", isUpgraded }
                };
                if (hasExhibit)
                {
                    storage.TryGetValue($"$exhibit{key}", out string exhibit);
                    tradeItem["Exhibit"] = exhibit;
                }
                trades[key] = tradeItem;
            }
            Helpers.AddDataValue("Trade", trades);
        }
    }
}