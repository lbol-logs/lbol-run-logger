using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using System.Collections.Generic;
using RunLogger.Utils.RunLogLib.Entities;
using LBoL.Core;

namespace RunLogger.Utils.Managers
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

        internal static void AddExhibitRewards(Exhibit exhibit)
        {
            StationReward reward = new StationReward()
            {
                Type = StationRewardType.Exhibit,
                Exhibit = exhibit
            };
            RewardsManager.AddReward(reward);
        }

        private static void AddEntitiesRewardsListItem<T>(string type, T listItem)
        {
            string key = type + "s";
            RewardsManager.GetRewards(out Dictionary<string, object> rewards);
            if (!rewards.TryGetValue(key, out object value)) rewards[key] = new List<T>();
            List<T> entitiesRewards = (value ?? rewards[key]) as List<T>;
            entitiesRewards.Add(listItem);
        }

        internal static void AddReward(StationReward reward)
        {
            RewardsManager.GetRewards(out Dictionary<string, object> rewards);
            StationRewardType rewardType = reward.Type;
            string type = rewardType.ToString();

            switch (rewardType)
            {
                case StationRewardType.Money:
                    rewards[type] = reward.Money;
                    break;
                case StationRewardType.Card:
                case StationRewardType.Tool:
                    List<CardObj> cards = Helpers.ParseCards(reward.Cards);
                    RewardsManager.AddEntitiesRewardsListItem(StationRewardType.Card.ToString(), cards);
                    break;
                case StationRewardType.Exhibit:
                    string exhibit = reward.Exhibit.Id;
                    RewardsManager.AddEntitiesRewardsListItem(type, exhibit);
                    break;
                default:
                    BepinexPlugin.log.LogDebug($"Reward of type `{type}` is ignored");
                    break;
            }
        }

        internal static void AddRewards(IEnumerable<StationReward> rewards)
        {
            foreach (StationReward reward in rewards) RewardsManager.AddReward(reward);
        }
    }
}