using HarmonyLib;
using LBoL.Base;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.EntityLib.Adventures.Shared23;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class NarumiOfferCardPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.RemoveDeckCards)), HarmonyPostfix]
        private static void AddType(IEnumerable<Card> cards)
        {
            if (!Helpers.IsAdventure<NarumiOfferCard>()) return;

            Card card = cards.First();
            string type = card.CardType == CardType.Misfortune ? CardType.Misfortune.ToString() : card.Config.Rarity.ToString();
            Helpers.AddDataValue("Type", type);
        }
    }
}