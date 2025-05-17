using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class CardDetailPanelPatch
    {
        [HarmonyPatch(typeof(CardDetailPanel), nameof(CardDetailPanel.Awake)), HarmonyPostfix]
        private static void CreateBackground(CardDetailPanel __instance)
        {
            if (Templates.Background != null) return;
            CardDetailPanel panel = __instance;
            Templates.Background = panel.transform.Find("SubWidgetGroup/TooltipParent/TooltipTemplate/Root/ExtraText").gameObject;
        }
    }
}