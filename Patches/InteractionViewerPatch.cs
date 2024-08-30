﻿using HarmonyLib;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(InteractionViewer))]
    public class InteractionViewerPatch
    {
        [HarmonyPatch(nameof(InteractionViewer.View)), HarmonyPrefix]
        static void ViewPatch(Interaction interaction)
        {
            if (interaction is MiniSelectCardInteraction)
            {
                if (RunDataController.CurrentStationIndex == 0 && interaction.Source == null)
                {
                    MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;
                    Dictionary<string, object> Rewards = new Dictionary<string, object>
                    {
                        { "Cards", RunDataController.GetCards(cards) }
                    };
                    RunDataController.CurrentStation.Rewards = Rewards;
                    return;
                }
            }

            if (interaction.Source == null) return;
            string source = interaction.Source.Id;

            if (interaction is RewardInteraction)
            {
                if (source == "HuiyeBaoxiang")
                {
                    RewardInteraction rewardInteraction = interaction as RewardInteraction;
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
            else if (interaction is MiniSelectCardInteraction)
            {
                if (source == "Modaoshu")
                {
                    MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
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
            }
        }
    }
}
