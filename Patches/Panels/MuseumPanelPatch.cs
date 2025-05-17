using HarmonyLib;
using LBoL.Core.StatusEffects;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class MuseumPanelPatch
    {
        [HarmonyPatch(typeof(MuseumPanel), nameof(MuseumPanel.Awake)), HarmonyPostfix]
        private static void CreateText(MuseumPanel __instance)
        {
            if (Templates.AutoUploadControl != null && Templates.AutoUploadText != null) return;
            BepinexPlugin.log.LogDebug("MuseumPanel Awaked");
            MuseumPanel panel = __instance;
            if (Templates.AutoUploadControl == null)
            {
                GameObject clone = Templates.Create(panel, "TabRoot/Cards/LeftScollView/Viewport/Content/ManaFilter");
                clone.name = Templates.Names.Control;
                foreach (Transform child in clone.transform) Object.Destroy(child.gameObject);
                Templates.AutoUploadControl = clone;
            }
            if (Templates.AutoUploadText == null)
            {
                GameObject clone = Templates.Create(panel, "TabRoot/Cards/LeftScollView/Viewport/Content/TextFilter/TextFilterInput/Text Area");
                clone.name = Templates.Names.Text;
                Templates.ChangeText(clone, "Placeholder", "Description (optional)");
                Templates.AutoUploadText = clone;
            }
        }
    }
}