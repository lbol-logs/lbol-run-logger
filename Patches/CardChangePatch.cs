using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.EntityLib.JadeBoxes;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyPatch]
    public static class CardChangePatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.InternalAddDeckCards)), HarmonyPostfix]
        private static void AddCardsPatch(Card[] cards)
        {
            EntitiesManager.AddCardChange(cards, ChangeType.Add);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.RemoveDeckCards)), HarmonyPrefix]
        static void RemoveCardsPatch(IEnumerable<Card> cards)
        {
            EntitiesManager.AddCardChange(cards, ChangeType.Remove);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.UpgradeDeckCards)), HarmonyPostfix]
        static void UpgradeCardsPatch(IEnumerable<Card> cards)
        {
            EntitiesManager.AddCardChange(cards, ChangeType.Upgrade);
        }
    }
}