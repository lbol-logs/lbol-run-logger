using HarmonyLib;
using LBoL.Base;
using LBoL.Core.SaveData;
using LBoL.Presentation;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyPatch(typeof(GameMaster))]
    class GameMasterPatch
    {
        [HarmonyPatch(nameof(GameMaster.AppendGameRunHistory)), HarmonyPostfix]
        static void AppendGameRunHistoryPatch(GameRunRecordSaveData record)
        {
            if (!RunDataController.Restore()) return;
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
            int ReloadTimes = record.ReloadTimes;
            string Seed = RandomGen.SeedToString(record.Seed);
            Result Result = new Result()
            {
                Type = Type,
                Timestamp = Timestamp,
                Cards = Cards,
                Exhibits = Exhibits,
                BaseMana = BaseMana,
                ReloadTimes = ReloadTimes,
                Seed = Seed
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
                $"{difficulty}{requests}",
                Type
            });
            RunDataController.Copy(name);

        }
    }
}