using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Exhibits.Common;
using Newtonsoft.Json;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

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
        static void Prefix(int value, Exhibit __instance)
        {
            if (RunDataController.isInitialize) return;

            string[] exhibits = { "GanzhuYao", "ChuRenou", "TiangouYuyi", "Moping", "Baota" };
            if (!exhibits.Contains(__instance.Id)) return;

            int before = __instance.Counter;

            if (before > value) RunDataController.AddExhibitChange(__instance, ChangeType.Use, value);
            else if (before < value) RunDataController.AddExhibitChange(__instance, ChangeType.Upgrade, value);
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
            if (RunDataController.RunData == null) return;
            RunDataController.RunData.Acts[0].Boss = enemyGroupName;
        }
    }
}
