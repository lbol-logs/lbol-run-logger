using LBoL.Core.Cards;
using LBoL.Core.Stations;
using System.Collections.Generic;

namespace RunLogger.Legacy.Utils
{
    public static class RewardsUtil
    {
        public static void AddReward(StationReward reward)
        {
            AddRewards(new List<StationReward> { reward });
        }

        public static void AddRewards(List<StationReward> rewards)
        {
            if (RunDataController.CurrentStation.Rewards == null) RunDataController.CurrentStation.Rewards = new Dictionary<string, object>();

            List<CardObj> Cards;
            foreach (StationReward reward in rewards)
            {
                StationRewardType Type = reward.Type;
                string type = Type.ToString();
                if (Type == StationRewardType.Money)
                {
                    int Money = reward.Money;
                    RunDataController.CurrentStation.Rewards[type] = Money;
                }
                else if (Type == StationRewardType.Card || Type == StationRewardType.Tool)
                {
                    List<Card> list = reward.Cards;
                    Cards = RunDataController.GetCards(list);
                    RunDataController.AddListItem2Obj(StationRewardType.Card.ToString(), Cards);
                }
                else if (Type == StationRewardType.Exhibit)
                {
                    string Exhibit = reward.Exhibit.Id;
                    RunDataController.AddListItem2Obj(type, Exhibit);
                }
            }
        }
    }
}
