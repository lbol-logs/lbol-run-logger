using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class CardDetailPanelPatch
    {
        [HarmonyPatch(typeof(CardDetailPanel), nameof(CardDetailPanel.Awake)), HarmonyPostfix]
        private static void AddBg(CardDetailPanel __instance)
        {
            if (ObjectsManager.Object.Bg != null) return;

            Transform panelT = ObjectsManager.Initialize();
            GameObject bg = ObjectsManager.Object.Bg = Object.Instantiate(
                __instance.transform.Find("SubWidgetGroup/TooltipParent/TooltipTemplate/Root/ExtraText").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                }
            );
            bg.SetActive(false);
            bg.name = "Bg";
            RectTransform bgT = bg.GetComponent<RectTransform>();
            bgT.pivot = new Vector2(0, 0.5f);
            bgT.anchorMin = new Vector2(0, 1);
            bgT.anchorMax = new Vector2(0, 1);
            bgT.sizeDelta = new Vector2(700, 100);
            Object.Destroy(bgT.Find("UpgradeText").gameObject);
            Object.Destroy(bgT.GetComponent<ContentSizeFitter>());
            ObjectsManager.ChangeText(bgT.Find("PoolText"), null);
        }
    }
}