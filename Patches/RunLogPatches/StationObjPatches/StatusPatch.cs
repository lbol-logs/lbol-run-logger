﻿using HarmonyLib;
using LBoL.Core.Units;
using LBoL.Core;
using RunLogger.Utils;
using LBoL.Presentation;
using LBoL.Core.SaveData;
using RunLogger.Utils.Enums;
using RunLogger.Utils.LogFile;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches
{
    [HarmonyPatch]
    internal static class StatusPatch
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
            if (!Instance.IsInitialized) return;

            GameRunController gameRun = __instance;
            PlayerUnit character = gameRun.Player;
            Controller.Instance.PreHealHp = character.Hp;
        }
    }
}