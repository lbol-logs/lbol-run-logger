using HarmonyLib;
using LBoL.Core;
using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.ExtraWidgets;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class GameResultPanelPatch
    {
        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnHiding)), HarmonyPostfix]
        private static void DestroyAllObjects()
        {
            BepinexPlugin.log.LogDebug("OnHiding");
            ObjectsManager.DestroyAllObjects();
        }
    }
}