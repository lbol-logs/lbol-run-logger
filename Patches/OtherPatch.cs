using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using RunLogger.Utils;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    class OtherPatch
    {
        [HarmonyPatch(typeof(DialogRunner), nameof(DialogRunner.SelectOption)), HarmonyPostfix]
        static void SelectOptionPatch(int id)
        {
            RunDataController.AddDataItem<int>("Choices", id);
        }
    }
}
