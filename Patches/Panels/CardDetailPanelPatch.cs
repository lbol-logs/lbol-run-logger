using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class CardDetailPanelPatch
    {
        [HarmonyPatch(typeof(CardDetailPanel), nameof(CardDetailPanel.Awake)), HarmonyPostfix]
        private static void CreateBackground(CardDetailPanel __instance)
        {
            if (Templates.AutoUploadWidget != null) return;
            BepinexPlugin.log.LogDebug("CardDetailPanel Awaked");
            CardDetailPanel panel = __instance;
            GameObject clone = Templates.Create(panel, "SubWidgetGroup/TooltipParent/TooltipTemplate/Root/ExtraText");

            GameObject widget = new GameObject(Templates.Names.Widget);
            widget.AddComponent<Image>();
            widget.AddComponent<HorizontalLayoutGroup>();
            // Copied fields can be restricted with BindingFlags
            {
                FieldInfo[] fields = clone.GetComponent<Image>().GetType().GetFields();
                foreach (FieldInfo field in fields) field.SetValue(widget, field.GetValue(clone));
            }
            Object.Destroy(clone);

            //Templates.RemoveChildren(clone, new[] { "UpgradeText", "PoolText" });


            //GameObject widget = Object.Instantiate(clone);
            //Object.Destroy(widget.GetComponent<Image>());
            //Object.Destroy(widget.GetComponent<VerticalLayoutGroup>());
            //widget.AddComponent<HorizontalLayoutGroup>();
            //Templates.AutoUploadWidget = widget;

            widget.name = Templates.Names.Widget;
            Templates.AutoUploadWidget = widget;
        }
    }
}