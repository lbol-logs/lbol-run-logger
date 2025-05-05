using HarmonyLib;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Exhibits.Shining;
using System.Collections.Generic;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class ExhibitsRewardsPatch
    {
        [HarmonyPatch(typeof(InteractionViewer), nameof(InteractionViewer.View)), HarmonyPrefix]
        private static void AddRewards(Interaction interaction)
        {
            string source = interaction.Source?.Id;
            if (source == null) return;

            if (interaction is RewardInteraction rewardInteraction)
            {
                if (source == nameof(HuiyeBaoxiang))
                {
                    IReadOnlyList<Exhibit> exhibits = rewardInteraction.PendingExhibits;

                    RewardsManager.GetRewards(out Dictionary<string, object> rewards);
                    if (!rewards.TryGetValue("Exhibits", out object value)) rewards["Exhibits"] = new List<string>();
                    List<string> exhibitsRewards = (value ?? rewards["Exhibits"]) as List<string>;
                    foreach (Exhibit exhibit in exhibits) exhibitsRewards.Add(exhibit.Id);
                }
            }
            else if (interaction is MiniSelectCardInteraction miniSelectCardInteraction)
            {
                if (source == nameof(Modaoshu))
                {
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;
                    RewardsManager.GetRewards(out Dictionary<string, object> rewards);
                    if (!rewards.TryGetValue("Cards", out object value)) rewards["Cards"] = new List<List<CardObj>>();
                    object cardsRewards = value ?? rewards["Cards"];

                    if (cardsRewards is List<List<CardObj>> cardsWithoutPrice)
                    {
                        cardsWithoutPrice.Add(new List<CardObj>());
                        cardsWithoutPrice[^1] = Helpers.ParseCards(cards);
                    }
                    else if (cardsRewards is List<List<CardObjWithPrice>> cardsWithPrice)
                    {
                        cardsWithPrice.Add(new List<CardObjWithPrice>());
                        cardsWithPrice[^1] = cards.Select(card => Helpers.ParseCardWithPrice(card)).ToList();
                    }
                }
                else if (source == nameof(FixBook))
                {
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;
                    List<List<CardObj>> cardsRewards = new List<List<CardObj>>() { Helpers.ParseCards(cards) };
                    RewardsManager.GetRewards(out Dictionary<string, object> rewards);
                    rewards["Cards"] = cardsRewards;
                }
            }
        }
    }
}