using HarmonyLib;
using LBoL.Core.Units;
using LBoL.Core;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib;
using LBoL.Presentation;
using LBoL.Core.SaveData;
using RunLogger.Utils.Enums;

namespace RunLogger.Patches.StationObjPatches
{
    [HarmonyPatch]
    public static class StatusPatch
    {
        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SaveGameRun)), HarmonyPrefix]
        private static void AddStatus(GameRunSaveData data, bool normalSave, GameMaster __instance)
        {
            bool toSave = Instance.IsInitialized && data.Timing == SaveTiming.EnterMapNode && normalSave;
            if (!toSave) return;

            BepinexPlugin.log.LogDebug("Add `Status`");
            GameRunController gameRun = __instance.CurrentGameRun;
            Helpers.AddStatus(gameRun, Controller.LastStation, Controller.Instance.PreHealHp);
            Controller.Instance.PreHealHp = null;

            Logger.SaveTemp(TempSaveTiming.EnterMapNode);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterNextStage)), HarmonyPrefix]
        private static void AddStatusFix(GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            PlayerUnit character = gameRun.Player;
            Controller.Instance.PreHealHp = character.Hp;
        }
    }
}