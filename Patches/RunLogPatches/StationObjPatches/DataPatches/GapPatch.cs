using HarmonyLib;
using LBoL.Core;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class GapPatch
    {
        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OnShowing)), HarmonyPostfix]
        private static void AddOptions(GapStation gapStation)
        {
            if (!Instance.IsInitialized) return;

            List<string> options = gapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
            Helpers.AddDataValue("Options", options);
        }

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        private static void AddChoice(GapOption option)
        {
            if (!Instance.IsInitialized) return;

            string choice = option.Type.ToString();
            Controller.CurrentStation.Data["Choice"] = choice;
        }

        //ShanliangDengpao
        [HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        private static void AddShanliangDengpao(SelectCardPayload payload)
        {
            if (!Instance.IsInitialized) return;

            bool isShanliangDengpao = Library.CreateGapOption<GetRareCard>().Name == payload.Name;
            if (!isShanliangDengpao) return;
            List<CardObj> cards = Helpers.ParseCards(payload.Cards);
            Helpers.AddDataValue("ShanliangDengpao", cards);
        }
    }
}