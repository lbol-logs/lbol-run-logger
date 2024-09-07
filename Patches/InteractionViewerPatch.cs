using HarmonyLib;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using RunLogger.Utils;
using System.Collections.Generic;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.Core.Stations;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(InteractionViewer))]
    public static class InteractionViewerPatch
    {
        [HarmonyPatch(nameof(InteractionViewer.View)), HarmonyPrefix]
        public static void ViewPatch(Interaction interaction)
        {
            if (interaction is MiniSelectCardInteraction)
            {
                if (RunDataController.CurrentStation.Type == StationType.Entry.ToString() && RunDataController.CurrentStation.Data.ContainsKey("Options"))
                {
                    AddMiniSelectCardInteractionRewards(interaction);
                    return;
                }
            }

            if (RunDataController.Listener == nameof(SumirekoGathering))
            {
                AddMiniSelectCardInteractionRewards(interaction);
            }
            else if (RunDataController.Listener == nameof(SatoriCounseling))
            {
                if (AdventurePatch.SatoriCounselingPatch.isMini) AddMiniSelectCardInteractionRewards(interaction);
                else AddSelectCardInteractionRewards(interaction);
            }
            RunDataController.Listener = null;

            if (interaction.Source == null) return;
            string source = interaction.Source.Id;

            if (interaction is RewardInteraction rewardInteraction)
            {
                if (source == nameof(HuiyeBaoxiang))
                {
                    IReadOnlyList<Exhibit> exhibits = rewardInteraction.PendingExhibits;

                    if (RunDataController.CurrentStation.Rewards == null)
                        RunDataController.CurrentStation.Rewards = new Dictionary<string, object>();
                    if (!RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out object value))
                        RunDataController.CurrentStation.Rewards["Exhibits"] = new List<string>();

                    RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out value);
                    List<string> currentExhibits = value as List<string>;
                    foreach (Exhibit exhibit in exhibits)
                    {
                        currentExhibits.Add(exhibit.Id);
                    }
                }
            }
            else if (interaction is MiniSelectCardInteraction miniSelectCardInteraction)
            {
                if (source == nameof(Modaoshu))
                {
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;

                    if (RunDataController.CurrentStation.Rewards == null)
                        RunDataController.CurrentStation.Rewards = new Dictionary<string, object>();
                    if (!RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out object value))
                        RunDataController.CurrentStation.Rewards["Cards"] = new List<List<CardObj>>();

                    RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out value);
                    List<List<CardObj>> currentCards = value as List<List<CardObj>>;
                    currentCards.Add(new List<CardObj>());
                    currentCards[^1] = RunDataController.GetCards(cards);
                }
                else if (source == nameof(FixBook))
                {
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;

                    List<List<CardObj>> Cards = new List<List<CardObj>>() { RunDataController.GetCards(cards) };
                    Dictionary<string, object> Rewards = new Dictionary<string, object>
                    {
                        { "Cards", Cards }
                    };
                    RunDataController.CurrentStation.Rewards = Rewards;
                }
            }
        }

        private static void AddMiniSelectCardInteractionRewards(Interaction interaction)
        {
            MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
            IReadOnlyList<Card> pendingCards = miniSelectCardInteraction.PendingCards;
            HandleCardRewards(pendingCards);
        }

        private static void AddSelectCardInteractionRewards(Interaction interaction)
        {
            SelectCardInteraction selectCardInteraction = interaction as SelectCardInteraction;
            IReadOnlyList<Card> pendingCards = selectCardInteraction.PendingCards;
            HandleCardRewards(pendingCards);
        }

        private static void HandleCardRewards(IReadOnlyList<Card> pendingCards)
        {
            List<CardObj> cards = RunDataController.GetCards(pendingCards);
            Dictionary<string, object> Rewards = new Dictionary<string, object>
            {
                { "Cards", new List<List<CardObj>>() { cards } }
            };
            RunDataController.CurrentStation.Rewards = Rewards;
        }
    }
}
