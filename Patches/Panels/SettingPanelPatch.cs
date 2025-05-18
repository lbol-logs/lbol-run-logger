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
            if (Templates.AutoUploadSwitch != null) return;
            BepinexPlugin.log.LogDebug("SettingPanel Awaked");
            SettingPanel panel = __instance;
            GameObject clone = Templates.Create(panel, "Root/Main/LeftPanel/AnimatingEnvironment/Switch");
            clone.name = Templates.Names.Switch;
            Templates.AutoUploadSwitch = clone;
        }
    }
}