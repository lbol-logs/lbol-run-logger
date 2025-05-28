using HarmonyLib;
using LBoL.EntityLib.Adventures.Shared12;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class YoumuDeliveryPatch
    {
        [HarmonyPatch(typeof(YoumuDelivery), nameof(YoumuDelivery.InitVariables)), HarmonyPostfix]
        private static void AddCard(YoumuDelivery __instance)
        {
            if (!Instance.IsInitialized) return;

            if (!Controller.ShowRandomResult) return;
            __instance.Storage.TryGetValue("$transformCard", out string card);
            Helpers.AddDataValue("Card", card);
        }
    }
}