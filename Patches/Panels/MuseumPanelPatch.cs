using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class MuseumPanelPatch
    {
        [HarmonyPatch(typeof(MuseumPanel), nameof(MuseumPanel.Awake)), HarmonyPostfix]
        private static void CreateText(MuseumPanel __instance)
        {
            if (Templates.AutoUploadText != null) return;
            BepinexPlugin.log.LogDebug("MuseumPanel Awaked");
            MuseumPanel panel = __instance;
            GameObject clone = Templates.Create(panel, "TabRoot/Cards/LeftScollView/Viewport/Content/TextFilter/TextFilterInput/Text Area");
            clone.name = Templates.Names.Text;
            Templates.ChangeText(clone, "Placeholder", "Description (optional)");
            //clone.transform.parent = panel.transform;
            clone.transform.SetParent(panel.transform, false);
            Templates.AutoUploadText = clone;
        }
    }
}