using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using UnityEngine;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class SettingPanelPatch
    {
        [HarmonyPatch(typeof(SettingPanel), nameof(SettingPanel.Awake)), HarmonyPostfix]
        private static void CreateSwitch(SettingPanel __instance)
        {
            if (Templates.Switch != null) return;
            SettingPanel panel = __instance;
            GameObject clone = Templates.CopyGameObject(panel, "Root/Main/LeftPanel/AnimatingEnvironment/Switch");
            clone.name = Templates.Names.Switch;
            Templates.Switch = clone;
        }
    }
}