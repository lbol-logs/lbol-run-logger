using HarmonyLib;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class DoremyPortalPatch
    {
        [HarmonyPatch(typeof(DoremyPortal), nameof(DoremyPortal.InitVariables)), HarmonyPostfix]
        private static void AddExhibit(DoremyPortal __instance)
        {
            if (!Controller.ShowRandomResult) return;
            __instance.Storage.TryGetValue("$randomExhibit", out string exhibit);
            Helpers.AddDataValue("Exhibit", exhibit);
        }
    }
}