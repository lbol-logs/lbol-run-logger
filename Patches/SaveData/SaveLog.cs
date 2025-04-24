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
using System.Reflection;

namespace RunLogger.Patches.SaveData
{
    [HarmonyPatch]
    public static class SaveLog
    {
        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.AppendGameRunHistory)), HarmonyPostfix]
        private static void AppendGameRunHistoryPatch(GameRunRecordSaveData record)
        {
            BepinexPlugin.log.LogDebug($"{MethodBase.GetCurrentMethod().DeclaringType} - START");
            SaveLog.SaveLogInternal(record);
            Logger.DeleteTemp();
            BepinexPlugin.log.LogDebug($"{MethodBase.GetCurrentMethod().DeclaringType} - END");
        }

        private static void SaveLogInternal(GameRunRecordSaveData record)
        {
            string resultType = record.ResultType.ToString();

            bool saveFailure = BepinexPlugin.saveFailure.Value;
            bool isFailure = resultType == GameResultType.Failure.ToString();
            bool toSave = saveFailure || !isFailure;

            if (!toSave) return;

            if (!Instance.IsInitialized)
            {
                RunLog runLog = Logger.LoadTemp();
                if (runLog == null) return;
                Controller.CreateInstance(runLog);
            }

            string timestamp = record.SaveTimestamp;
            List<CardObj> cards = record.Cards.Select(card =>
            {
                return new CardObj()
                {
                    Id = card.Id,
                    IsUpgraded = card.Upgraded,
                    UpgradeCounter = card.UpgradeCounter
                };
            }).ToList();
            List<string> exhibits = record.Exhibits.ToList();
            string baseMana = Controller.GetBaseMana(record.BaseMana, exhibits);
            int reloadTimes = record.ReloadTimes;
            string seed = RandomGen.SeedToString(record.Seed);
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
            string character = record.Player;
            string parsedType = record.PlayerType.ToString().Replace("Type", "");
            string shining = exhibits[0];
            Settings settings = Controller.Instance.RunLog.Settings;
            char difficulty = settings.Difficulty[0];
            int requests = settings.Requests.Count;

            string filename = string.Join("_", new string[]
            {
                parsedTimestamp,
                character,
                parsedType,
                shining,
                $"{difficulty}{requests}",
                resultType
            });

            Logger.SaveLog(filename);
        }
    }
}