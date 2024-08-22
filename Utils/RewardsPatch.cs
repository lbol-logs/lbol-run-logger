using LBoL.Core.Cards;
using LBoL.Core.Stations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunLogger.Utils
{
    public static class RewardsPatch
    {
        public static void GenerateRewards<T>(T __instance)
        {
            List<StationReward> rewards = (__instance as BattleStation).Rewards;
            Dictionary<string, object> Rewards = new Dictionary<string, object>();
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
                    List<CardObj> Cards = RunDataController.GetCards(list);
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
                Rewards = RunDataController.CurrentStation.Rewards.Concat(Rewards).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            RunDataController.CurrentStation.Rewards = Rewards;
        }

        public static void AddReward(StationReward reward)
        {
            Dictionary<string, object> Rewards = new Dictionary<string, object>();

            string Type = reward.Type.ToString();
            Debugger.Write(Type);
            if (Type == "Money")
            {
                int Money = reward.Money;
                Rewards[Type] = Money;
            }
            else if (Type == "Card" || Type == "Tool")
            {
                List<Card> list = reward.Cards;
                List<CardObj> Cards = RunDataController.GetCards(list);
                RunDataController.AddListItem2Obj(ref Rewards, Type, Cards);
            }
            else if (Type == "Exhibit")
            {
                string Exhibit = reward.Exhibit.Id;
                RunDataController.AddListItem2Obj(ref Rewards, Type, Exhibit);
            }

            if (RunDataController.CurrentStation.Rewards != null)
            {
                Rewards = RunDataController.CurrentStation.Rewards.Concat(Rewards).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            RunDataController.CurrentStation.Rewards = Rewards;
        }
    }
}
