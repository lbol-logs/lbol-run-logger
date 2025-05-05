using HarmonyLib;
using LBoL.Core.Stations;
using LBoL.Core;
using RunLogger.Legacy.Patches;
using RunLogger.Legacy.Utils;
using static System.Collections.Specialized.BitVector32;
using System.Collections.Generic;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class RewardsPatch
    {
        [HarmonyPatch(typeof(Stage), nameof(Stage.GetEliteEnemyExhibit)), HarmonyPostfix]
        //YizangnuoWuzhi
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
            RewardsUtil.AddReward(reward);
        }
    }
}