﻿using HarmonyLib;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class RemiliaMeetPatch
    {
        [HarmonyPatch(typeof(RemiliaMeet), nameof(RemiliaMeet.InitVariables)), HarmonyPostfix]
        private static void AddHasExhibit(RemiliaMeet __instance)
        {
            if (!Instance.IsInitialized) return;

            __instance.Storage.TryGetValue("$hasExhibit", out bool hasExhibit);
            Helpers.AddDataValue("HasExhibit", hasExhibit);
        }
    }
}