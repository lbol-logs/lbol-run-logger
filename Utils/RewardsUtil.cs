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
                string Type = reward.Type.ToString();
                if (Type == "Money")
                {
                    int Money = reward.Money;
                    Rewards[Type] = Money;
                }
                else if (Type == "Card" || Type == "Tool")
                {
                    List<Card> list = reward.Cards;
                    Cards = RunDataController.GetCards(list);
                    RunDataController.AddListItem2Obj(ref Rewards, Type, Cards);
                }
                else if (Type == "Exhibit")
                {
                    string Exhibit = reward.Exhibit.Id;
                    RunDataController.AddListItem2Obj(ref Rewards, Type, Exhibit);
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
