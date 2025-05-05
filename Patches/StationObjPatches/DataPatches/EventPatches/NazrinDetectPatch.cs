using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class NazrinDetectPatch
    {
        [HarmonyPatch(typeof(NazrinDetectPanel), nameof(NazrinDetectPanel.Roll)), HarmonyPostfix]
        private static void AddResult(int resultIndex)
        {
            Helpers.AddDataValue("Result", resultIndex);
        }
    }
}