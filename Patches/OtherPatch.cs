using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Stations;
using LBoL.EntityLib.Exhibits.Common;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Counter), MethodType.Setter)]
    class ExihibitCounterSetterPatch
    {
        static void Prefix(int value, Exhibit __instance)
        {
            if (RunDataController.isInitialize) return;

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

    [HarmonyDebug]
    [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Initialize))]
    class ExihibitInitializePatch
    {
        static void Prefix()
        {
            RunDataController.isInitialize = true;
        }

        static void Postfix()
        {
            RunDataController.isInitialize = false;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(Stage))]
    public static class StagePatch
    {
        public static bool waitForSave = false;

        [HarmonyPatch(nameof(Stage.SetBoss)), HarmonyPostfix]
        static void SetBossPatch(string enemyGroupName)
        {
            if (RunDataController.RunData == null) return;
            RunDataController.RunData.Acts[0].Boss = enemyGroupName;
        }

        [HarmonyPatch(nameof(Stage.GetEnemyCardReward)), HarmonyPostfix]
        static void GetEnemyCardRewardPatch(StationReward __result)
        {
            if (!waitForSave) return;
            RewardsUtil.AddReward(__result);
        }

        [HarmonyPatch(typeof(StationReward), nameof(StationReward.CreateToolCard)), HarmonyPostfix]
        static void GetShopToolCardsPatch(StationReward __result)
        {
            if (RunDataController.Listener != null) return;
            RewardsUtil.AddReward(__result);
        }
    }
}
