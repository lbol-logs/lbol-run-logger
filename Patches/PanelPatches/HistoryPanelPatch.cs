using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanelObjects;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class HistoryPanelPatch
    {
        [HarmonyPatch(typeof(HistoryPanel), nameof(HistoryPanel.Awake)), HarmonyPostfix]
        private static void CreateStatus(HistoryPanel __instance)
        {
            if (UploadPanel.HasPanel || ObjectsManager.GetFromTemp("Status") != null) return;

            RectTransform status = ObjectsManager.CopyGameObject(__instance.transform, "RecordDataArea/SeedButton");
            status.name = "Status";
            status.pivot = new Vector2(0.5f, 0.5f);
            status.position = PositionsManager.StatusPosition;

            RectTransform text = status.Find("SeedText").GetComponent<RectTransform>();
            ObjectsManager.ChangeText(text, null);
            TextMeshProUGUI textMeshProUGUI = text.GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.fontSize = 60;
            textMeshProUGUI.fontSizeMax = 60;
            text.pivot = new Vector2(0, 0.5f);
            text.localPosition = Vector3.zero;
            UploadPanel.AdjustPanel();
        }
    }
}