using HarmonyLib;
using LBoL.Core.Dialogs;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class DialogPatch
    {
        //[HarmonyPatch(typeof(DialogOption), nameof(DialogOption.GetLocalizedText)), HarmonyPostfix]
        //private static void HandleChoices(string ____lineId)
        //{
        //    switch (____lineId)
        //    {
        //        case AdventurePatch.BackgroundDancersPatch.line:
        //            AdventurePatch.BackgroundDancersPatch.HandleOptions();
        //            break;
        //    }
        //}
    }
}