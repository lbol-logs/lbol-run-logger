using HarmonyLib;
using LBoL.Core.Stations;
using LBoL.Core;
using System.Collections.Generic;
using RunLogger.Utils;
using System;

namespace RunLogger.Patches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class RewardsPatch
    {
        [HarmonyPatch(typeof(Station), nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) }), HarmonyPrefix]
        private static void AddRewards(IEnumerable<StationReward> rewards)
        {
            RewardsManager.AddRewards(rewards);
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

            StationReward reward = new StationReward()
            {
                Type = StationRewardType.Exhibit,
                Exhibit = exhibit
            };
            RewardsManager.AddReward(reward);
        }
    }
}