using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Common;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class PrismriverChaosPatch
    {
        [HarmonyPatch(typeof(PrismriverChaos), nameof(PrismriverChaos.InitVariables))]
        [HarmonyPatch(typeof(PrismriverChaos), nameof(PrismriverChaos.Tolerate))]
        [HarmonyPostfix]
        private static void AddData(PrismriverChaos __instance)
        {
            if (!Instance.IsInitialized) return;

            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$damage", out float hp);
            Helpers.AddDataListItem("Hps", (int)hp);

            storage.TryGetValue("$card", out string id);
            storage.TryGetValue("$isUpgraded", out bool isUpgraded);
            CardObj cardObj = new CardObj()
            {
                Id = id,
                IsUpgraded = isUpgraded
            };
            Helpers.AddDataListItem("Cards", cardObj);
        }
    }
}