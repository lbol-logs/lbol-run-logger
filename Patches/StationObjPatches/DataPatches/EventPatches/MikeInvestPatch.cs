using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Shared12;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class MikeInvestPatch
    {
        [HarmonyPatch(typeof(MikeInvest), nameof(MikeInvest.InitVariables)), HarmonyPostfix]
        private static void AddData(MikeInvest __instance)
        {
            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$longMoney", out float money);
            Helpers.AddDataValue("Money", (int)money);

            if (!Controller.ShowRandomResult) return;
            storage.TryGetValue("$cardReward", out string card);
            Helpers.AddDataValue("Card", card);
        }
    }
}