using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanelObjects;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class ProfilePanelPatch
    {
        [HarmonyPatch(typeof(ProfilePanel), nameof(ProfilePanel.Awake)), HarmonyPostfix]
        private static void CreateTextArea(ProfilePanel __instance)
        {
            if (UploadPanel.HasPanel || ObjectsManager.GetFromTemp("TextArea") != null) return;

            RectTransform textArea = ObjectsManager.CopyGameObject(__instance.transform, "NameInput");
            textArea.name = "TextArea";
            textArea.gameObject.SetActive(false);

            Object.Destroy(textArea.Find("InputField").gameObject);
            ObjectsManager.ChangeText(textArea.Find("Title"), "Description");
            Transform confirmT = textArea.Find("Confirm");
            ObjectsManager.ChangeText(confirmT.Find("Layout/Text (TMP)"), "Upload");


            RectTransform edit = ObjectsManager.CopyGameObject(__instance.transform, "Profiles/Layout/ProfileWidget0/Content/EditButton", ObjectsManager.GetFromTemp("Upload"));
            edit.name = "Edit";
            edit.localPosition = Vector3.zero;
            UploadPanel.AdjustPanel();
        }
    }
}