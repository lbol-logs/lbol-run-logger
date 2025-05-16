using HarmonyLib;
using LBoL.Core.Stats;
using LBoL.Core;
using RunLogger.Utils;
using LBoL.Core.SaveData;
using LBoL.Presentation;
using RunLogger.Utils.Enums;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class RoundsPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddRounds(BattleStats __result)
        {
            BattleStats battleStats = __result;
            int rounds = battleStats.TotalRounds;
            Helpers.AddDataValue("Rounds", rounds);
        }

        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SaveGameRun)), HarmonyPrefix]
        private static void SaveTemp(GameRunSaveData data, bool normalSave)
        {
            bool toSave = Instance.IsInitialized && data.Timing == SaveTiming.BattleFinish && normalSave;
            if (!toSave) return;
            Logger.SaveTemp(TempSaveTiming.BattleFinish);
        }
    }
}