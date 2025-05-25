using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    //[HarmonyPatch]
    internal static class HistoryPanelPatch
    {
        [HarmonyPatch(typeof(HistoryPanel), nameof(HistoryPanel.Awake)), HarmonyPostfix]
        private static void CreateStatus(HistoryPanel __instance)
        {
            if (ObjectsManager.Object.Status != null) return;

            Transform panelT = ObjectsManager.Initialize();
            GameObject status = ObjectsManager.Object.Status = Object.Instantiate(
                __instance.transform.Find("RecordDataArea/SeedButton").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                }
            );
            status.SetActive(false);
            status.name = "Status";
            RectTransform statusT = status.GetComponent<RectTransform>();
            statusT.pivot = new Vector2(0.5f, 0.5f);

            RectTransform textT = statusT.Find("SeedText").GetComponent<RectTransform>();
            ObjectsManager.ChangeText(textT, null);
            TextMeshProUGUI textMeshProUGUI = textT.GetComponent<TextMeshProUGUI>();
            textMeshProUGUI.fontSize = 60;
            textMeshProUGUI.fontSizeMax = 60;
            textT.pivot = new Vector2(0, 0.5f);
            textT.localPosition = Vector3.zero;
        }
    }
}