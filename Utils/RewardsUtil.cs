using LBoL.Core.Cards;
using LBoL.Core.Stations;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Utils
{
    public static class RewardsUtil
    {
        public static void AddReward(StationReward reward)
        {
            AddRewards(new List<StationReward> { reward });
        }

        public static void AddRewards(List<StationReward> rewards)
        {
            Dictionary<string, object> Rewards = new Dictionary<string, object>();
            List<CardObj> Cards = null;
            foreach (StationReward reward in rewards)
            {
                StationRewardType Type = reward.Type;
                string type = Type.ToString();
                if (Type == StationRewardType.Money)
                {
                    int Money = reward.Money;
                    Rewards[type] = Money;
                }
                else if (Type == StationRewardType.Card || Type == StationRewardType.Tool)
                {
                    List<Card> list = reward.Cards;
                    Cards = RunDataController.GetCards(list);
                    RunDataController.AddListItem2Obj(ref Rewards, type, Cards);
                }
                else if (Type == StationRewardType.Exhibit)
                {
                    string Exhibit = reward.Exhibit.Id;
                    RunDataController.AddListItem2Obj(ref Rewards, type, Exhibit);
                }
            }
            if (RunDataController.CurrentStation.Rewards != null)
            {
                if (Cards != null && RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out object currentCards))
                {
                    (currentCards as List<List<CardObj>>).Add(Cards);
                }
                else
                {
                    RunDataController.CurrentStation.Rewards = RunDataController.CurrentStation.Rewards.Concat(Rewards).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
            }
            else
            {
                RunDataController.CurrentStation.Rewards = Rewards;
            }
        }
    }
}
