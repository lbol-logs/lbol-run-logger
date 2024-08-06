﻿using HarmonyLib;
using LBoL.Core.SaveData;
using LBoL.Presentation;
using Newtonsoft.Json;
using RunLogger.Utils;
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
            List<CardObj> Cards = JsonConvert.DeserializeObject<List<CardObj>>(JsonConvert.SerializeObject(record.Cards));
            List<string> Exhibits = record.Exhibits.ToList();
            Result Result = new Result()
            {
                Type = Type,
                Timestamp = Timestamp,
                Cards = Cards,
                Exhibits = Exhibits
            };
            RunDataController.RunData.Result = Result;
            RunDataController.Save();

            string ts = Timestamp.Replace(":", "-");
            string character = record.Player;
            string type = record.PlayerType.ToString().Replace("Type", "");

            string name = $"{ts}_{character}_{type}_{Result}";
            RunDataController.Copy(name);

        }
    }
}
