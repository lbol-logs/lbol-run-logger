using HarmonyLib;
using LBoL.Core.Stations;
using LBoL.Core;
using System.Collections.Generic;
using RunLogger.Utils;
using System;
using LBoL.EntityLib.Adventures;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class RewardsPatch
    {
        [HarmonyPatch(typeof(Station), nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) }), HarmonyPrefix]
        private static void AddRewards(IEnumerable<StationReward> rewards)
        {
            bool isDebut = Helpers.IsAdventure<Debut>();
            if (isDebut)
            {
                if (Controller.CurrentStation == null)
                {
                    List<IEnumerable<StationReward>> rewardsBeforeDebut = Controller.Instance.RewardsBeforeDebut ??= new List<IEnumerable<StationReward>>();
                    rewardsBeforeDebut.Add(rewards);
                }
                return;
            }

            RewardsManager.AddRewards(rewards);
        }

        [HarmonyPatch(typeof(BossStation), nameof(BossStation.GenerateBossRewards)), HarmonyPostfix]
        private static void AddBossExhibits(BossStation __instance)
        {
            foreach (Exhibit exhibit in __instance.BossRewards) RewardsManager.AddExhibitRewards(exhibit);
        }

        //JimuWanju
        [HarmonyPatch(typeof(Stage), nameof(Stage.GetEnemyCardReward)), HarmonyPostfix]
        private static void AddExtraCardsReward(StationReward __result)
        {
            Station currentStation = Helpers.CurrentStation;
            if (!(currentStation is EnemyStation)) return;
            bool isGenerateEnemyRewards = currentStation.Rewards.Count == 0;
            if (isGenerateEnemyRewards) return;

            RewardsManager.AddReward(__result);
        }

        //YizangnuoWuzhi
        [HarmonyPatch(typeof(Stage), nameof(Stage.GetEliteEnemyExhibit)), HarmonyPostfix]
        private static void AddExtraExhibitReward(Exhibit __result)
        {
            Exhibit exhibit = __result;
            Station currentStation = Helpers.CurrentStation;
            if (!(currentStation is EliteEnemyStation)) return;
            bool isGenerateEliteEnemyRewards = currentStation.Rewards.Count == 0;
            if (isGenerateEliteEnemyRewards) return;

            RewardsManager.AddExhibitRewards(exhibit);
        }

        //Gongjuxiang
        [HarmonyPatch(typeof(StationReward), nameof(StationReward.CreateToolCard)), HarmonyPostfix]
        private static void GetShopToolCardsPatch(StationReward __result)
        {
            Station currentStation = Helpers.CurrentStation;
            if (!(currentStation is BattleStation)) return;

            RewardsManager.AddReward(__result);
        }
    }
}