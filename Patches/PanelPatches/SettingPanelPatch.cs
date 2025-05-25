using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.UploadPanel;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class SettingPanelPatch
    {
        private static GameObject Go;

        [HarmonyPatch(typeof(SettingPanel), nameof(SettingPanel.Awake)), HarmonyPostfix]
        private static void CreateStatus(SettingPanel __instance)
        {
            GameObject go = ObjectsManager.Object.Status = Object.Instantiate(__instance.transform.Find("Background").gameObject);
            BepinexPlugin.log.LogDebug(ObjectsManager.Object.Status.name);
            BepinexPlugin.log.LogDebug($"Status: {ObjectsManager.Object.Status ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($"go: {go ?? (object)"null"}");
            //Object.Destroy(ObjectsManager.Object.Status);
            //BepinexPlugin.log.LogDebug(ObjectsManager.Object.Status.name);
            Object.Destroy(go);
            BepinexPlugin.log.LogDebug($"go: {go ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($"go is null: {go == null}");
            return;
            BepinexPlugin.log.LogDebug($"Default: {default(GameObject) ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($"Property: {Go ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($"RectTransform: {Go?.GetComponent<RectTransform>() ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($"Get Transform: {Go?.GetComponent<Transform>() ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($".transform: {Go?.transform ?? (object)"null"}");

            BepinexPlugin.log.LogDebug($"Status: {ObjectsManager.Object.Status ?? (object)"null"}");
            Transform statusT = ObjectsManager.Object.Status?.transform;
            BepinexPlugin.log.LogDebug($"statusT: {statusT ?? (object)"null"}");
            BepinexPlugin.log.LogDebug($"before");
            if (statusT != null)
            {
                BepinexPlugin.log.LogDebug($"inner");
                statusT.position = Vector3.zero;
            }
            BepinexPlugin.log.LogDebug($"after");
        }
    }
}