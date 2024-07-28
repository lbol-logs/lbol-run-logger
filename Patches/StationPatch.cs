using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using Newtonsoft.Json;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(BattleStation))]
    class BattleStationPatch
    {
        [HarmonyPatch(typeof(EnemyStation), nameof(EnemyStation.GenerateRewards)), HarmonyPostfix]
        static void GenerateRewardsPatch_Enemy(EnemyStation __instance)
        {
            GenerateRewardsPatch(__instance);
        }
        [HarmonyPatch(typeof(EliteEnemyStation), nameof(EliteEnemyStation.GenerateRewards)), HarmonyPostfix]
        static void GenerateRewardsPatch_Elite(EliteEnemyStation __instance)
        {
            GenerateRewardsPatch(__instance);
        }
        [HarmonyPatch(typeof(BossStation), nameof(BossStation.GenerateRewards)), HarmonyPostfix]
        static void GenerateRewardsPatch_Boss(BossStation __instance)
        {
            GenerateRewardsPatch(__instance);
        }
        static void GenerateRewardsPatch<T>(T __instance)
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
                    List<CardObj> Cards = list.Select(card =>
                    {
                        CardObj Card = new CardObj
                        {
                            Id = card.Id,
                            IsUpgraded = card.IsUpgraded,
                            UpgradeCounter = card.UpgradeCounter
                        };
                        return Card;
                    }).ToList();
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
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(BossStation))]
    class BossStationPatch
    {
        [HarmonyPatch(nameof(BossStation.GenerateBossRewards)), HarmonyPostfix]
        static void GenerateBossRewardsPatch(BossStation __instance)
        {
            Exhibit[] BossRewards = __instance.BossRewards;
            List<string> Exhibits = BossRewards.Select(exhibit => exhibit.Id).ToList();
            Dictionary<string, object> Rewards = new Dictionary<string, object>
            {
                { "Exhibits", Exhibits }
            };
            RunDataController.CurrentStation.Rewards = Rewards;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(GapStation))]
    class GapStation
    {
        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        static void OptionClickedPatch(GapOption option, GapOptionsPanel __instance)
        {
            string Choice = option.Type.ToString();
            List<string> Options = __instance.GapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
            RunDataController.AddData("Choice", Choice);
            RunDataController.AddData("Options", Options);
        }
    }
}