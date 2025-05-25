using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanel;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    //[HarmonyPatch]
    internal static class ProfilePanelPatch
    {
        [HarmonyPatch(typeof(ProfilePanel), nameof(ProfilePanel.Awake)), HarmonyPostfix]
        private static void CreateTextArea(ProfilePanel __instance)
        {
            if (ObjectsManager.Object.TextArea != null) return;

            Transform panelT = ObjectsManager.Initialize();
            GameObject textArea = ObjectsManager.Object.TextArea = Object.Instantiate(
                __instance.transform.Find("NameInput").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                }
            );
            textArea.SetActive(false);
            textArea.name = "TextArea";
            Transform textAreaT = textArea.transform;

            Object.Destroy(textAreaT.Find("InputField").gameObject);

            ObjectsManager.ChangeText(textAreaT.Find("Title"), "Description");

            Transform confirmT = textAreaT.Find("Confirm");
            ObjectsManager.ChangeText(confirmT.Find("Layout/Text (TMP)"), "Upload");
            ObjectsManager.SetClickEvent(confirmT, () =>
            {
                textArea.SetActive(false);
                BepinexPlugin.log.LogDebug(ObjectsManager.Text);
                //LBoLLogs.Upload(ObjectsManager.Text);
            });
            ObjectsManager.SetClickEvent(textAreaT.Find("Cancel"), () =>
            {
                textArea.SetActive(false);
                ObjectsManager.Text = null;
            });

            GameObject edit = ObjectsManager.Object.Edit = Object.Instantiate(
                __instance.transform.Find("Profiles/Layout/ProfileWidget0/Content/EditButton").gameObject,
                new InstantiateParameters
                {
                    parent = ObjectsManager.Object.Upload.transform,
                    worldSpace = true
                }
            );
            edit.SetActive(false);
            edit.name = "Edit";
            ObjectsManager.SetTooltip(edit, "Add description", "optional");
            RectTransform editT = edit.GetComponent<RectTransform>();
            ObjectsManager.SetClickEvent(editT, () =>
            {
                textArea.SetActive(true);
            });
            editT.localPosition = Vector3.zero;
        }
    }
}