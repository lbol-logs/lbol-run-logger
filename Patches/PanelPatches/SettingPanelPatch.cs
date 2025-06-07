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
            if (UploadPanel.HasPanel || ObjectsManager.GetFromTemp("AutoUpload") != null) return;

            RectTransform autoUpload = ObjectsManager.CopyGameObject(__instance.transform, "Root/Main/LeftPanel/AnimatingEnvironment");
            autoUpload.name = "AutoUpload";
            autoUpload.position = PositionsManager.AutoUploadPosition;

            Transform label = autoUpload.Find("KeyTmp");
            label.name = "Label";
            ObjectsManager.ChangeText(label, "Auto Upload");

            RectTransform switchT = autoUpload.Find("Switch").GetComponent<RectTransform>();
            switchT.localPosition = new Vector3(switchT.localPosition.x + PositionsManager.SwitchOffsetX, switchT.localPosition.y, switchT.localPosition.z);
            UploadPanel.AdjustPanel();
        }
    }
}