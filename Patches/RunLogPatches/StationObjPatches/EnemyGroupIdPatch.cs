using HarmonyLib;
using LBoL.Core.Units;
using LBoL.Core;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches
{
    [HarmonyPatch]
    internal static class EnemyGroupIdPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddEnemyGroupId(EnemyGroup enemyGroup, GameRunController __instance)
        {
            if (!Instance.IsInitialized) return;

            GameRunController gameRun = __instance;
            string adventureId = Helpers.GetAdventureId(gameRun.CurrentStation);
            if (adventureId != null) Helpers.AddDataValue("Id", enemyGroup.Id);
        }
    }
}