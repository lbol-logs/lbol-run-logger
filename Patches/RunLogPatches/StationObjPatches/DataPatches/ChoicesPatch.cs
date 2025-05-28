using HarmonyLib;
using LBoL.Core.Dialogs;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class ChoicesPatch
    {
        [HarmonyPatch(typeof(DialogRunner), nameof(DialogRunner.SelectOption)), HarmonyPostfix]
        private static void AddChoices(int id)
        {
            if (!Instance.IsInitialized) return;
            Helpers.AddDataListItem("Choices", id);
        }
    }
}