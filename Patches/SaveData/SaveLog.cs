using HarmonyLib;
using LBoL.Base;
using LBoL.Core.SaveData;
using LBoL.Core;
using LBoL.Presentation;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils;
using System.Collections.Generic;
using RunLogger.Utils.RunLogLib.Entities;
using System.Linq;
using RunLogger.Utils.LogFile;

namespace RunLogger.Patches.SaveData
{
    [HarmonyPatch]
    internal static class SaveLog
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.AbandonGameRun)), HarmonyPostfix]
        private static void SetIsAbandoned()
        {
            Controller.Instance.IsAbandoned = true;
        }

        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SaveProfileWithEndingGameRun)), HarmonyPostfix]
        private static void EndRun(GameRunController gameRun, GameRunRecordSaveData gameRunRecord)
        {
            BepinexPlugin.log.LogDebug("End run");
            SaveLog.EndRunInternal(gameRun, gameRunRecord);
            Logger.DeleteTemp();
        }

        private static void EndRunInternal(GameRunController gameRun, GameRunRecordSaveData gameRunRecord)
        {
            if (!Instance.IsInitialized) return;

            if (Controller.Instance.IsAbandoned && !BepinexPlugin.SaveAbandoned.Value) return;

            Helpers.AddStatus(gameRun, Controller.CurrentStation, null);

            string resultType = gameRunRecord.ResultType.ToString();

            bool saveFailure = BepinexPlugin.SaveFailure.Value;
            bool isFailure = resultType == GameResultType.Failure.ToString();
            bool toSave = saveFailure || !isFailure;

            if (!toSave) return;

            string timestamp = gameRunRecord.SaveTimestamp;
            List<CardObj> cards = gameRunRecord.Cards.Select(card =>
            {
                return new CardObj()
                {
                    Id = card.Id,
                    IsUpgraded = card.Upgraded,
                    UpgradeCounter = card.UpgradeCounter
                };
            }).ToList();
            List<string> exhibits = gameRunRecord.Exhibits.ToList();
            string baseMana = Helpers.GetBaseMana(gameRunRecord.BaseMana, exhibits);
            int reloadTimes = gameRunRecord.ReloadTimes;
            string seed = RandomGen.SeedToString(gameRunRecord.Seed);
            Result result = new Result()
            {
                Type = resultType,
                Timestamp = timestamp,
                Cards = cards,
                Exhibits = exhibits,
                BaseMana = baseMana,
                ReloadTimes = reloadTimes,
                Seed = seed
            };
            Controller.Instance.RunLog.Result = result;

            string parsedTimestamp = timestamp.Replace(":", "-");
            string character = gameRunRecord.Player;
            string parsedType = gameRunRecord.PlayerType.ToString().Replace("Type", "");
            string shining = exhibits[0];
            Settings settings = Controller.Instance.RunLog.Settings;
            char difficulty = settings.Difficulty[0];
            int requests = settings.Requests.Count;

            string name = string.Join("_", new string[]
            {
                parsedTimestamp,
                character,
                parsedType,
                shining,
                $"{difficulty}{requests}",
                resultType
            });

            Logger.SaveLog(name);
        }
    }
}