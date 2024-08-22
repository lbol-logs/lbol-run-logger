using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using Newtonsoft.Json;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Station))]
    class StationPatch
    {
        [HarmonyPatch(nameof(Station.AddReward)), HarmonyPostfix]
        static void AddRewardPatch(StationReward reward)
        {
            RewardsPatch.AddReward(reward);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(BossStation))]
    class BossStationPatch
    {
        [HarmonyPatch(nameof(BossStation.GenerateBossRewards)), HarmonyPostfix]
        static void GenerateBossRewardsPatch(BossStation __instance)
        {
            Exhibit[] BossRewards = __instance.BossRewards;
            List<string> Exhibits = BossRewards.Select(exhibit => exhibit.Id).ToList();
            Dictionary<string, object> Rewards = new Dictionary<string, object>
            {
                { "Exhibits", Exhibits }
            };
            RunDataController.CurrentStation.Rewards = Rewards;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(GapStation))]
    class GapStation
    {
        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        static void OptionClickedPatch(GapOption option, GapOptionsPanel __instance)
        {
            string Choice = option.Type.ToString();
            List<string> Options = __instance.GapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
            RunDataController.AddData("Choice", Choice);
            RunDataController.AddData("Options", Options);
        }

        [HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        static void ShowMiniSelectPatch(SelectCardPayload payload)
        {
            string name = payload.Name;
            switch (name)
            {
                case "GetRareCard":
                    List<CardObj> cards = RunDataController.GetCards(payload.Cards);
                    RunDataController.AddData("ShanliangDengpao", cards);
                    break;
            }
        }
    }
}