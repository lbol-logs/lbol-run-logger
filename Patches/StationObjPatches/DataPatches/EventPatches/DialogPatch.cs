using HarmonyLib;
using LBoL.Core.Dialogs;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class DialogPatch
    {
        //[HarmonyPatch(typeof(DialogLinePhase), nameof(DialogLinePhase.GetLocalizedText)), HarmonyPostfix]
        //private static void HandleDialogues(string ____lineId)
        //{
        //    switch (____lineId)
        //    {
        //        case AdventurePatch.BuduSuanmingPatch.lineBoss:
        //            RunDataController.AddData("Boss", AdventurePatch.BuduSuanmingPatch.boss);
        //            break;
        //        case AdventurePatch.BuduSuanmingPatch.lineEvent:
        //            RunDataController.AddData("Host", AdventurePatch.BuduSuanmingPatch.host);
        //            break;
        //        case AdventurePatch.BuduSuanmingPatch.lineExhibit:
        //            RunDataController.AddData("Exhibit", AdventurePatch.BuduSuanmingPatch.exhibit);
        //            break;
        //    }
        //}

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