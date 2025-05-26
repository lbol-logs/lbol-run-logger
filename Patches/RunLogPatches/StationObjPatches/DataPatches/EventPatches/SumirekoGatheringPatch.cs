using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.Core;
using LBoL.EntityLib.Adventures;
using RunLogger.Utils;
using System.Collections.Generic;
using LBoL.Core.Battle;
using RunLogger.Utils.RunLogLib.Entities;
using RunLogger.Utils.Managers;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class SumirekoGatheringPatch
    {
        [HarmonyPatch(typeof(SumirekoGathering), nameof(SumirekoGathering.InitVariables)), HarmonyPostfix]
        private static void AddData(SumirekoGathering __instance)
        {
            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$rareCard1", out string id);
            if (id == null) return;

            storage.TryGetValue("$isUpgraded", out bool isUpgraded);
            CardObj cardObj = new CardObj()
            {
                Id = id,
                IsUpgraded = isUpgraded
            };
            Helpers.AddDataValue("Card", cardObj);
            SumirekoGatheringPatch.AddCards(storage);
        }

        [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.HasMoney)), HarmonyPostfix]
        private static void AddHasMoney(bool __result)
        {
            if (!Helpers.IsAdventure<SumirekoGathering>(out DialogStorage storage)) return;

            bool hasMoney = __result;
            Helpers.AddDataValue("HasMoney", hasMoney);
            if (!hasMoney) return;

            Helpers.GetData(out Dictionary<string, object> data);
            if (data.ContainsKey("Cards")) return;

            SumirekoGatheringPatch.AddCards(storage);
        }

        [HarmonyPatch(typeof(InteractionViewer), nameof(InteractionViewer.View)), HarmonyPrefix]
        private static void AddCardsRewards(Interaction interaction)
        {
            if (!Helpers.IsAdventure<SumirekoGathering>()) return;
            RewardsManager.AddCardsRewards(interaction);
        }

        private static void AddCards(DialogStorage storage)
        {
            List<string> cards = Helpers.GetStorageList<string, int>(storage, new int[] { 1, 2, 3 }, "$rareTrade");
            Helpers.AddDataValue("Cards", cards);
        }
    }
}