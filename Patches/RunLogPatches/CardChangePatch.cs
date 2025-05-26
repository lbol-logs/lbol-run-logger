using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using RunLogger.Utils.Managers;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Patches.RunLogPatches
{
    [HarmonyPatch]
    internal static class CardChangePatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.InternalAddDeckCards)), HarmonyPostfix]
        private static void AddCardsPatch(Card[] cards)
        {
            EntitiesManager.AddCardChange(cards, ChangeType.Add);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.RemoveDeckCards)), HarmonyPrefix]
        private static void RemoveCardsPatch(IEnumerable<Card> cards)
        {
            EntitiesManager.AddCardChange(cards, ChangeType.Remove);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.UpgradeDeckCards)), HarmonyPostfix]
        private static void UpgradeCardsPatch(IEnumerable<Card> cards)
        {
            EntitiesManager.AddCardChange(cards, ChangeType.Upgrade);
        }
    }
}