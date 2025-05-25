using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    //[HarmonyPatch]
    internal static class MuseumPanelPatch
    {
        [HarmonyPatch(typeof(MuseumPanel), nameof(MuseumPanel.Awake)), HarmonyPostfix]
        private static void AddInput(MuseumPanel __instance)
        {
            if (ObjectsManager.Object.Input != null) return;

            Transform panelT = ObjectsManager.Initialize();
            GameObject input = ObjectsManager.Object.Input = Object.Instantiate(
                __instance.transform.Find("TabRoot/Cards/LeftScollView/Viewport/Content/TextFilter").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                }
            );
            input.SetActive(false);
            input.name = "Input";
            RectTransform inputT = input.GetComponent<RectTransform>();
            Object.Destroy(inputT.Find("ClearTextFilter").gameObject);
            inputT.localPosition = new Vector3(0, 100, 0);
            inputT.sizeDelta = new Vector2(2000, 700);

            TMP_InputField tmpInput = ObjectsManager.TmpInput;
            tmpInput.onValueChanged = null;
            tmpInput.lineType = TMP_InputField.LineType.MultiLineNewline;
            tmpInput.characterLimit = 300;

            RectTransform boxT = inputT.Find("TextFilterInput").GetComponent<RectTransform>();
            boxT.position = new Vector3(0, -9.4f, 10);

            Transform textAreaT = boxT.Find("Text Area");
            ObjectsManager.ChangeText(textAreaT.Find("Placeholder"), "optional");
            textAreaT.Find("Text").GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.TopLeft;
        }
    }
}