using HarmonyLib;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
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

            Transform inputFieldT = textAreaT.Find("InputField");
            TMP_InputField tmp = inputFieldT.GetComponent<TMP_InputField>();
            tmp.text = null;
            tmp.lineType = TMP_InputField.LineType.MultiLineNewline;
            tmp.onValidateInput = null;
            tmp.characterLimit = 300;
            Object.Destroy(tmp.GetComponent<CharNumTransf>());

            ObjectsManager.ChangeText(tmp.transform.Find("ViewPort/Text"), null);

            ObjectsManager.ChangeText(textAreaT.Find("Title"), "Description");

            Transform confirmT = textAreaT.Find("Confirm");
            ObjectsManager.ChangeText(confirmT.Find("Layout/Text (TMP)"), "Upload");
            ObjectsManager.SetClickEvent(confirmT, () =>
            {
                textArea.SetActive(false);
                LBoLLogs.Upload(tmp.text);
            });
            ObjectsManager.SetClickEvent(textAreaT.Find("Cancel"), () =>
            {
                textArea.SetActive(false);
                tmp.text = null;
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
            ObjectsManager.SetClickEvent(edit.transform, () =>
            {
                textArea.SetActive(true);
                //textArea.GetComponent<CanvasGroup>().interactable = true;
            });
            //TODO bg height and width
            //TODO horizontal wrap, vertical truncate
        }
    }
}