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
            if (Templates.Text != null) return;
            MuseumPanel panel = __instance;
            GameObject clone = Templates.CopyGameObject(panel, "TabRoot/Cards/LeftScollView/Viewport/Content/TextFilter/TextFilterInput");
            clone.name = Templates.Names.Text;
            Templates.ChangeText(clone.transform.Find("Text Area").gameObject, "Placeholder", "Description (optional)");
            Templates.Text = clone;
        }
    }
}