using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Exhibits.Common;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

    [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Counter), MethodType.Setter)]
    class ExihibitCounterSetterPatch
    {
        static void Postfix(int value, Exhibit __instance)
        {
            if (RunDataController.isInitialize) return;
            if (__instance is ChuRenou ChuRenou)
            {
                RunDataController.AddExhibitUse(__instance, value);
            }
            else if (__instance is GanzhuYao GanzhuYao)
            {
                RunDataController.AddExhibitUse(__instance, value);
            }
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            return instructions;
        }
    }

    [HarmonyPatch(typeof(Exhibit), nameof(Exhibit.Initialize))]
    class ExihibitInitializePatch
    {
        static void Prefix()
        {
            RunDataController.isInitialize = true;
        }

        static void Postfix()
        {
            RunDataController.isInitialize = false;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(Stage))]
    class StagePatch
    {
        [HarmonyPatch(nameof(Stage.SetBoss)), HarmonyPostfix]
        static void SetBossPatch(string enemyGroupName)
        {
            RunDataController.RunData.Acts[0].Boss = enemyGroupName;
        }
    }
}
