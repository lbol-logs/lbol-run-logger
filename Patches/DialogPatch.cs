using HarmonyLib;
using LBoL.Core.Dialogs;
using RunLogger.Utils;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    class DialogPatch
    {
        [HarmonyDebug]
        [HarmonyPatch(typeof(DialogRunner))]
        class DialogRunnerPatch
        {
            [HarmonyPatch(nameof(DialogRunner.SelectOption)), HarmonyPostfix]
            static void SelectOptionPatch(int id, string ____name)
            {
                RunDataController.AddDataItem("Choices", id);
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(DialogFunctions))]
        class DialogFunctionsPatch
        {
            [HarmonyPatch(nameof(DialogFunctions.AdventureRand)), HarmonyPostfix]
            static void AdventureRandPatch(int __result)
            {
                RunDataController.AddDataItem("Values", __result);
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(DialogLinePhase))]
        class DialogLinePhasePatch
        {
            [HarmonyPatch(nameof(DialogLinePhase.GetLocalizedText)), HarmonyPostfix]
            static void GetLocalizedTextPatch(string ____lineId)
            {
                switch (____lineId)
                {
                    case AdventurePatch.SatoriCounselingPatch.lineValid:
                        RunDataController.AddData("HasMoney", true);
                        break;
                    case AdventurePatch.SatoriCounselingPatch.lineInvalid:
                        RunDataController.AddData("HasMoney", false);
                        break;
                    case AdventurePatch.BuduSuanmingPatch.lineBoss:
                        RunDataController.AddData("Boss", AdventurePatch.BuduSuanmingPatch.boss);
                        break;
                    case AdventurePatch.BuduSuanmingPatch.lineEvent:
                        RunDataController.AddData("Host", AdventurePatch.BuduSuanmingPatch.host);
                        break;
                    case AdventurePatch.BuduSuanmingPatch.lineExhibit:
                        RunDataController.AddData("Exhibit", AdventurePatch.BuduSuanmingPatch.exhibit);
                        break;
                }
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(DialogOption))]
        class DialogPatchPatch
        {
            [HarmonyPatch(nameof(DialogOption.GetLocalizedText)), HarmonyPostfix]
            static void GetLocalizedTextPatch(string ____lineId)
            {
                switch (____lineId)
                {
                    case AdventurePatch.BackgroundDancersPatch.line:
                        AdventurePatch.BackgroundDancersPatch.HandleOptions();
                        break;
                }
            }
        }
    }
}