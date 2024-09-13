using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Stations;
using LBoL.EntityLib.Exhibits.Common;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static RunLogger.Patches.StationPatch;

namespace RunLogger.Patches
{
    [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Counter), MethodType.Setter)]
    class ExihibitCounterSetterPatch
    {
        public static bool isInitialize;
        static void Prefix(int value, Exhibit __instance)
        {
            if (isInitialize) return;

            string[] exhibits = { nameof(GanzhuYao), nameof(ChuRenou), nameof(TiangouYuyi), nameof(Moping), nameof(Baota) };
            if (!exhibits.Contains(__instance.Id)) return;

            int before = __instance.Counter;

            if (before > value) RunDataController.AddExhibitChange(__instance, ChangeType.Use, value);
            else if (before < value) RunDataController.AddExhibitChange(__instance, ChangeType.Upgrade, value);
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions;
        }
    }

    [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Initialize))]
    class ExihibitInitializePatch
    {
        static void Prefix()
        {
            ExihibitCounterSetterPatch.isInitialize = true;
        }

        static void Postfix()
        {
            ExihibitCounterSetterPatch.isInitialize = false;
        }
    }

    [HarmonyPatch(typeof(Stage))]
    public static class StagePatch
    {
        [HarmonyPatch(nameof(Stage.SetBoss)), HarmonyPostfix]
        static void SetBossPatch(string enemyGroupName)
        {
            if (RunDataController.RunData == null) return;
            RunDataController.RunData.Acts[0].Boss = enemyGroupName;
        }

        [HarmonyPatch(nameof(Stage.GetEnemyCardReward)), HarmonyPostfix]
        static void GetEnemyCardRewardPatch(StationReward __result)
        {
            bool isAfterAddRewards = StationPatch.AddRewardsPatch.isAfterAddRewards;
            BepinexPlugin.log.LogDebug($"isAfterAddRewards: {isAfterAddRewards}");
            if (!isAfterAddRewards) return;
            RewardsUtil.AddReward(__result);
        }

        [HarmonyPatch(typeof(StationReward), nameof(StationReward.CreateToolCard)), HarmonyPostfix]
        static void GetShopToolCardsPatch(StationReward __result)
        {
            string RewardListener = StationPatch.RewardListener;
            BepinexPlugin.log.LogDebug($"RewardListener in {System.Reflection.MethodBase.GetCurrentMethod().Name}: {RewardListener}");
            if (RewardListener != null) return;
            RewardsUtil.AddReward(__result);
        }
    }
}
