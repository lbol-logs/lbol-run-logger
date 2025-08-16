using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanelObjects;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class ProfilePanelPatch
    {
        [HarmonyPatch(typeof(ProfilePanel), nameof(ProfilePanel.Awake)), HarmonyPostfix]
        private static void CreateTextArea(ProfilePanel __instance)
        {
            if (UploadPanel.SkipPanelTemp || ObjectsManager.GetFromTemp("TextArea") != null) return;

            RectTransform textArea = ObjectsManager.CopyGameObject(__instance.transform, "NameInput");
            textArea.name = "TextArea";
            textArea.gameObject.SetActive(false);

            Transform input = textArea.Find("InputField");
            RectTransform count = ObjectsManager.CopyGameObject(input, "ViewPort/Text", textArea);
            count.name = "Count";
            count.pivot = new Vector2(1, 0.5f);
            count.localPosition = PositionsManager.CountLocalPosition;
            count.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
            ObjectsManager.ChangeText(count, "0/300");

            Object.Destroy(input.gameObject);

            ObjectsManager.ChangeText(textArea.Find("Title"), "Description");
            Transform confirm = textArea.Find("Confirm");
            ObjectsManager.ChangeText(confirm.Find("Layout/Text (TMP)"), "Upload");

            RectTransform edit = ObjectsManager.CopyGameObject(__instance.transform, "Profiles/Layout/ProfileWidget0/Content/EditButton", ObjectsManager.GetFromTemp("Upload"));
            edit.name = "Edit";
            edit.localPosition = Vector3.zero;
            UploadPanel.AdjustPanel();
        }
    }
}