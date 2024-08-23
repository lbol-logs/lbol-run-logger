using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.SaveData;
using LBoL.Presentation;
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
            string BaseMana = record.BaseMana;
            foreach (string Exhibit in Exhibits)
            {
                ExhibitConfig config = ExhibitConfig.FromId(Exhibit);
                Rarity rarity = config.Rarity;
                if (rarity != Rarity.Shining) continue;
                ManaColor? manaColor = config.BaseManaColor;
                if (manaColor == null) BaseMana += "A";
            }
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

            string name = $"{ts}_{character}_{type}_{Type}";
            RunDataController.Copy(name);

        }
    }
}
