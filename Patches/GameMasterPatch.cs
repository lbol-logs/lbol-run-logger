using HarmonyLib;
using LBoL.Core.SaveData;
using LBoL.Presentation;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameMaster))]
    class GameMasterPatch
    {
        [HarmonyPatch(nameof(GameMaster.AppendGameRunHistory)), HarmonyPostfix]
        static void AppendGameRunHistoryPatch(GameRunRecordSaveData record)
        {
            RunDataController.Restore();
            string Type = record.ResultType.ToString();
            string Timestamp = record.SaveTimestamp;
            List<CardObj> Cards = record.Cards.Select(card =>
            {
                return new CardObj()
                {
                    Id = card.Id,
                    IsUpgraded = card.Upgraded,
                    UpgradeCounter = card.UpgradeCounter
                };
            }).ToList();
            List<string> Exhibits = record.Exhibits.ToList();
            string BaseMana = RunDataController.GetBaseMana(record.BaseMana, Exhibits);
            Result Result = new Result()
            {
                Type = Type,
                Timestamp = Timestamp,
                Cards = Cards,
                Exhibits = Exhibits,
                BaseMana = BaseMana
            };
            RunDataController.RunData.Result = Result;
            RunDataController.Save();

            string ts = Timestamp.Replace(":", "-");
            string character = record.Player;
            string type = record.PlayerType.ToString().Replace("Type", "");
            string shining = Exhibits[0];
            Settings settings = RunDataController.RunData.Settings;
            char difficulty = settings.Difficulty[0];
            int requests = settings.Requests.Count;

            string name = String.Join("_", new string[]
            {
                ts,
                character,
                type,
                shining,
                difficulty.ToString(),
                requests.ToString(),
                Type
            });
            RunDataController.Copy(name);

        }
    }
}