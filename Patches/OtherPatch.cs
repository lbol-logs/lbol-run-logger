using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using LBoL.Core.Units;
using RunLogger.Utils;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(DialogRunner))]
    class DialogRunnerPatch
    {
        [HarmonyPatch(nameof(DialogRunner.SelectOption)), HarmonyPostfix]
        static void SelectOptionPatch(int id)
        {
            RunDataController.AddDataItem<int>("Choices", id);
        }
    }

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
