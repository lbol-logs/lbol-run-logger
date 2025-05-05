using HarmonyLib;
using LBoL.Core.Stations;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class OpponentsPatch
    {
        [HarmonyPatch(typeof(SelectStation), nameof(SelectStation.GenerateRecord)), HarmonyPostfix]
        private static void AddOpponents(SelectStation __instance)
        {
            List<string> opponents = __instance.Opponents.Select(opponent => opponent.Id).ToList();
            Helpers.AddDataValue("opponents", opponents);
        }
    }
}