using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanelObjects;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class MuseumPanelPatch
    {
        [HarmonyPatch(typeof(MuseumPanel), nameof(MuseumPanel.Awake)), HarmonyPostfix]
        private static void CreateInput(MuseumPanel __instance)
        {
            if (UploadPanel.HasPanel || ObjectsManager.GetFromTemp("Input") != null) return;

            RectTransform input = ObjectsManager.CopyGameObject(__instance.transform, "TabRoot/Cards/LeftScollView/Viewport/Content/TextFilter");
            input.name = "Input";
            Object.Destroy(input.Find("ClearTextFilter").gameObject);
            input.localPosition = PositionsManager.InputLocalPosition;
            input.sizeDelta = PositionsManager.InputSizeDelta;

            TMP_InputField tmpInput = input.Find("TextFilterInput").GetComponent<TMP_InputField>();
            tmpInput.onValueChanged = null;
            tmpInput.lineType = TMP_InputField.LineType.MultiLineNewline;
            tmpInput.characterLimit = 300;

            RectTransform box = input.Find("TextFilterInput").GetComponent<RectTransform>();
            box.position = PositionsManager.BoxPosition;

            Transform textArea = box.Find("Text Area");
            ObjectsManager.ChangeText(textArea.Find("Placeholder"), "optional");
            textArea.Find("Text").GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
            UploadPanel.AdjustPanel();
        }
    }
}