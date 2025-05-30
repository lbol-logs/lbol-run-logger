﻿using HarmonyLib;
using LBoL.EntityLib.Adventures.Stage1;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class AssistKagerouPatch
    {
        [HarmonyPatch(typeof(AssistKagerou), nameof(AssistKagerou.InitVariables)), HarmonyPostfix]
        private static void AddExhibit(AssistKagerou __instance)
        {
            if (!Instance.IsInitialized) return;

            if (!Controller.ShowRandomResult) return;
            __instance.Storage.TryGetValue("$exhibitReward", out string exhibit);
            Helpers.AddDataValue("Exhibit", exhibit);
        }
    }
}