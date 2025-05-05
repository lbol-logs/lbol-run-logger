using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using System.Collections.Generic;
using RunLogger.Utils.RunLogLib.Entities;

namespace RunLogger.Utils
{
    internal static class RewardsManager
    {
        internal static void GetRewards(out Dictionary<string, object> rewards)
        {
            rewards = Controller.CurrentStation.Rewards ??= new Dictionary<string, object>();
        }

        internal static void AddCardsRewards(Interaction interaction)
        {
            IReadOnlyList<Card> pendingCards;
            if (interaction is MiniSelectCardInteraction miniSelectCardInteraction) pendingCards = miniSelectCardInteraction.PendingCards;
            else pendingCards = (interaction as SelectCardInteraction).PendingCards;
            List<CardObj> cardObjs = Helpers.ParseCards(pendingCards);
            RewardsManager.GetRewards(out Dictionary<string, object> rewards);
            rewards.Add("Cards", new List<List<CardObj>>() { cardObjs });
        }

        private static void AddEntitiesRewardsListItem<T>(string type, T listItem)
        {
            string key = type + "s";
            RewardsManager.GetRewards(out Dictionary<string, object> rewards);
            if (!rewards.TryGetValue(key, out object value)) rewards[key] = new List<T>();
            List<T> entitiesRewards = (value ?? rewards[key]) as List<T>;
            entitiesRewards.Add(listItem);
        }
    }
}
