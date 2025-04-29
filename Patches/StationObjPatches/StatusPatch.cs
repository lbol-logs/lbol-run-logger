using HarmonyLib;
using LBoL.Core.Units;
using LBoL.Core;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib;
using LBoL.Presentation;
using LBoL.Core.SaveData;

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

            StationObj currentStation = Controller.CurrentStation;
            GameRunController gameRun = __instance.CurrentGameRun;
            PlayerUnit character = gameRun.Player;
            int hp;
            if (Controller.Instance.PreHealHp != null)
            {
                hp = (int)Controller.Instance.PreHealHp;
                Controller.Instance.PreHealHp = null;
            }
            else
            {
                hp = character.Hp;
            }

            Status status = new Status
            {
                Money = gameRun.Money,
                Hp = hp,
                MaxHp = character.MaxHp,
                Power = character.Power,
                MaxPower = character.MaxPower
            };

            if (currentStation == null) Controller.Instance.RunLog.Settings.Status = status;
            else currentStation.Status = status;

            Logger.SaveTemp();
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