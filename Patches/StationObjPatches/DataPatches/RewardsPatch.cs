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
        [HarmonyPatch(typeof(Station), nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) })]
        private static void AddRewards(IEnumerable<StationReward> rewards)
        {
            RewardsManager.AddRewards(rewards);
        }

        //YizangnuoWuzhi
        [HarmonyPatch(typeof(Stage), nameof(Stage.GetEliteEnemyExhibit)), HarmonyPostfix]
        private static void AddExtraExhibitReward(Exhibit __result)
        {
            Exhibit exhibit = __result;
            int exhibits = Helpers.CurrentStation.Rewards.Count;
            if (exhibits == 0) return;

            StationReward reward = new StationReward()
            {
                Type = StationRewardType.Exhibit,
                Exhibit = exhibit
            };
            RewardsManager.AddReward(reward);
        }
    }
}