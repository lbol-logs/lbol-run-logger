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
    }
}