using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanelObjects;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class SettingPanelPatch
    {
        [HarmonyPatch(typeof(SettingPanel), nameof(SettingPanel.Awake)), HarmonyPostfix, HarmonyPriority(Priority.High)]
        private static void CreateAutoUpload(SettingPanel __instance)
        {
            if (UploadPanel.SkipPanelTemp || ObjectsManager.GetFromTemp("AutoUpload") != null) return;

            RectTransform autoUpload = ObjectsManager.CopyGameObject(__instance.transform, "Root/Main/LeftPanel/AnimatingEnvironment");
            autoUpload.name = "AutoUpload";
            autoUpload.position = PositionsManager.AutoUploadPosition;

            Transform label = autoUpload.Find("KeyTmp");
            label.name = "Label";
            label.position = new Vector3(PositionsManager.LabelPositionX, label.position.y, label.position.z);
            ObjectsManager.ChangeText(label, "Auto Upload");

            RectTransform switchT = autoUpload.Find("Switch").GetComponent<RectTransform>();
            switchT.position = new Vector3(PositionsManager.SwitchPositionX, switchT.position.y, switchT.position.z);
            UploadPanel.AdjustPanel();
        }
    }
}