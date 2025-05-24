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

            RectTransform inputFieldT = textAreaT.Find("InputField").GetComponent<RectTransform>();
            TMP_InputField tmpInput = inputFieldT.GetComponent<TMP_InputField>();
            tmpInput.text = null;
            tmpInput.lineType = TMP_InputField.LineType.MultiLineNewline;
            tmpInput.onValidateInput = null;
            tmpInput.characterLimit = 300;
            inputFieldT.offsetMin = new Vector2(-1000, -80);
            inputFieldT.offsetMax = new Vector2(1000, 80);
            Object.Destroy(tmpInput.GetComponent<CharNumTransf>());

            RectTransform textT = tmpInput.transform.Find("ViewPort/Text").GetComponent<RectTransform>();
            ObjectsManager.ChangeText(textT, null);
            textT.offsetMin = Vector2.zero;
            textT.offsetMax = Vector2.zero;
            TextMeshProUGUI tmp = textT.GetComponent<TextMeshProUGUI>();
            tmp.enableAutoSizing = false;
            tmp.alignment = TextAlignmentOptions.TopLeft;

            ObjectsManager.ChangeText(textAreaT.Find("Title"), "Description");

            Transform confirmT = textAreaT.Find("Confirm");
            ObjectsManager.ChangeText(confirmT.Find("Layout/Text (TMP)"), "Upload");
            ObjectsManager.SetClickEvent(confirmT, () =>
            {
                textArea.SetActive(false);
                LBoLLogs.Upload(tmpInput.text);
            });
            ObjectsManager.SetClickEvent(textAreaT.Find("Cancel"), () =>
            {
                textArea.SetActive(false);
                tmpInput.text = null;
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

            //TODO bg height and width
            //TODO horizontal wrap, vertical truncate
        }
    }
}