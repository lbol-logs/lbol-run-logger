using HarmonyLib;
using LBoL.Core;
using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.ExtraWidgets;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
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
            if (ObjectsManager.Objects.TextArea != null) return;

            Transform panelT = ObjectsManager.Initialize();
            GameObject textArea = ObjectsManager.Objects.TextArea = Object.Instantiate(
                __instance.transform.Find("NameInput").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                }
            );
            textArea.name = "TextArea";
            //textArea.SetActive(false);

            Transform inputFieldT = textArea.transform.Find("InputField");
            TMP_InputField tmp = inputFieldT.GetComponent<TMP_InputField>();
            tmp.text = null;
            tmp.lineType = TMP_InputField.LineType.MultiLineNewline;
            tmp.onValidateInput = null;
            tmp.characterLimit = 300;
            Object.Destroy(tmp.GetComponent<CharNumTransf>());
//TODO bg height and width
//TODO horizontal wrap, vertical truncate
        }
    }
}