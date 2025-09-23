using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanelObjects;
using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class CardDetailPanelPatch
    {
        [HarmonyPatch(typeof(CardDetailPanel), nameof(CardDetailPanel.Awake)), HarmonyPostfix]
        private static void CreateBg(CardDetailPanel __instance)
        {
            if (UploadPanel.SkipPanelTemp || ObjectsManager.GetFromTemp("Bg") != null) return;

            RectTransform bg = ObjectsManager.CopyGameObject(__instance.transform, "SubWidgetGroup/TooltipParent/TooltipTemplate/Root/ExtraText", wrapped: true);
            bg.name = "Bg";
            bg.pivot = new Vector2(0, 0.5f);
            bg.anchorMin = new Vector2(0, 1);
            bg.anchorMax = new Vector2(0, 1);
            bg.sizeDelta = new Vector2(700, 100);
            bg.position = PositionsManager.BgPosition;
            Object.Destroy(bg.Find("UpgradeText").gameObject);
            Object.Destroy(bg.GetComponent<ContentSizeFitter>());
            Object.Destroy(bg.Find("PackText")?.gameObject);
            ObjectsManager.ChangeText(bg.Find("PoolText"), null);
            UploadPanel.AdjustPanel();
        }
    }
}