using HarmonyLib;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using System.Collections.Generic;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.Core.Stations;
using System.Linq;
using RunLogger.Legacy.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch(typeof(InteractionViewer))]
    public static class InteractionViewerPatch
    {
        //public static string Listener;

        //[HarmonyPatch(nameof(InteractionViewer.View)), HarmonyPrefix]
        //public static void ViewPatch(Interaction interaction)
        //{
        //    if (Listener != null) BepinexPlugin.log.LogDebug($"InteractionViewerPatch.Listener: {Listener}");

        //    if (interaction is MiniSelectCardInteraction)
        //    {
        //        if (RunDataController.CurrentStation.Type == StationType.Entry.ToString() && AdventurePatch.DebutPatch.uncommonCardListener)
        //        {
        //            AddMiniSelectCardInteractionRewards(interaction);
        //            AdventurePatch.DebutPatch.uncommonCardListener = false;
        //            return;
        //        }
        //    }

        //    switch (Listener)
        //    {
        //        case nameof(SumirekoGathering):
        //            AddMiniSelectCardInteractionRewards(interaction);
        //            break;
        //        case nameof(SatoriCounseling):
        //            if (AdventurePatch.SatoriCounselingPatch.isMini) AddMiniSelectCardInteractionRewards(interaction);
        //            else AddSelectCardInteractionRewards(interaction);
        //            break;
        //    }
        //    Listener = null;

        //    if (interaction.Source == null) return;
        //    string source = interaction.Source.Id;

        //    if (interaction is RewardInteraction rewardInteraction)
        //    {
        //        if (source == nameof(HuiyeBaoxiang))
        //        {
        //            IReadOnlyList<Exhibit> exhibits = rewardInteraction.PendingExhibits;

        //            if (RunDataController.CurrentStation.Rewards == null)
        //                RunDataController.CurrentStation.Rewards = new Dictionary<string, object>();
        //            if (!RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out object value))
        //                RunDataController.CurrentStation.Rewards["Exhibits"] = new List<string>();

        //            RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out value);
        //            List<string> currentExhibits = value as List<string>;
        //            foreach (Exhibit exhibit in exhibits)
        //            {
        //                currentExhibits.Add(exhibit.Id);
        //            }
        //        }
        //    }
        //    else if (interaction is MiniSelectCardInteraction miniSelectCardInteraction)
        //    {
        //        if (source == nameof(Modaoshu))
        //        {
        //            IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;
        //            if (RunDataController.CurrentStation.Rewards == null)
        //                RunDataController.CurrentStation.Rewards = new Dictionary<string, object>();
        //            if (!RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out object value))
        //                RunDataController.CurrentStation.Rewards["Cards"] = new List<List<CardObj>>();

        //            var cardsRewards = RunDataController.CurrentStation.Rewards["Cards"];
        //            if (cardsRewards is List<List<CardObj>> cardsWithoutPrice)
        //            {
        //                cardsWithoutPrice.Add(new List<CardObj>());
        //                cardsWithoutPrice[^1] = RunDataController.GetCards(cards);
        //            }
        //            else if (cardsRewards is List<List<CardWithPrice>> cardsWithPrice)
        //            {
        //                cardsWithPrice.Add(new List<CardWithPrice>());
        //                cardsWithPrice[^1] = cards.Select(card => RunDataController.GetCardWithPrice(card)).ToList();
        //            }
        //        }
        //        else if (source == nameof(FixBook))
        //        {
        //            IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;

        //            List<List<CardObj>> Cards = new List<List<CardObj>>() { RunDataController.GetCards(cards) };
        //            Dictionary<string, object> Rewards = new Dictionary<string, object>
        //            {
        //                { "Cards", Cards }
        //            };
        //            RunDataController.CurrentStation.Rewards = Rewards;
        //        }
        //    }
        //}

        //private static void AddMiniSelectCardInteractionRewards(Interaction interaction)
        //{
        //    MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
        //    IReadOnlyList<Card> pendingCards = miniSelectCardInteraction.PendingCards;
        //    HandleCardRewards(pendingCards);
        //}

        //private static void AddSelectCardInteractionRewards(Interaction interaction)
        //{
        //    SelectCardInteraction selectCardInteraction = interaction as SelectCardInteraction;
        //    IReadOnlyList<Card> pendingCards = selectCardInteraction.PendingCards;
        //    HandleCardRewards(pendingCards);
        //}

        //private static void HandleCardRewards(IReadOnlyList<Card> pendingCards)
        //{
        //    List<CardObj> cards = RunDataController.GetCards(pendingCards);
        //    Dictionary<string, object> Rewards = new Dictionary<string, object>
        //    {
        //        { "Cards", new List<List<CardObj>>() { cards } }
        //    };
        //    RunDataController.CurrentStation.Rewards = Rewards;
        //}
    }
}
