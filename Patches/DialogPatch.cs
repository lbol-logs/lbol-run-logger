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
                if (!Debugger.isDebug) return;
                Debugger.Write(____lineId);
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(DialogOption))]
        class DialogPatchPatch
        {
            [HarmonyPatch(nameof(DialogOption.GetLocalizedText)), HarmonyPostfix]
            static void GetLocalizedTextPatch(string ____lineId, DialogOption __instance)
            {
                if (!Debugger.isDebug) return;
                Debugger.Write(__instance.Id.ToString() + ": " + ____lineId);
            }
        }
    }
}
