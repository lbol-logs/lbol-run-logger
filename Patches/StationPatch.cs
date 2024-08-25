using HarmonyLib;
using LBoL.Core;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Station))]
    class StationPatch
    {
        [HarmonyPatch(nameof(Station.AddReward)), HarmonyPrefix]
        static void AddRewardPatch(StationReward reward)
        {
            RunDataController.Listener = "AddReward";
            RewardsUtil.AddReward(reward);
            RunDataController.Listener = null;
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
    class GapStationPatch
    {
        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        static void OptionClickedPatch(GapOption option, GapOptionsPanel __instance)
        {
            string Choice = option.Type.ToString();
            List<string> Options = __instance.GapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
            RunDataController.AddData("Choice", Choice);
            RunDataController.AddData("Options", Options);
        }

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.InternalGerRareCard)), HarmonyPrefix]
        static void InternalGerRareCardPatch()
        {
            RunDataController.Listener = "GetRareCard";
        }

        [HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        static void ShowMiniSelectPatch(SelectCardPayload payload)
        {
            switch (RunDataController.Listener)
            {
                case "GetRareCard":
                    List<CardObj> cards = RunDataController.GetCards(payload.Cards);
                    RunDataController.AddData("ShanliangDengpao", cards);
                    break;
            }
            RunDataController.Listener = null;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(SelectStation))]
    class SelectStationPatch
    {
        [HarmonyPatch(nameof(SelectStation.GenerateRecord)), HarmonyPostfix]
        static void GenerateRecordPatch(SelectStation __instance)
        {
            List<string> Opponents = __instance.Opponents.Select(opponent =>  opponent.Id).ToList();
            RunDataController.AddData("Opponents", Opponents);
        }
    }
}