using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class GameResultPanelPatch
    {
        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.Awake)), HarmonyPostfix]
        private static void AppendWidget(GameResultPanel __instance)
        {
            if (Templates.HasAppened) return;
            BepinexPlugin.log.LogDebug("GameResultPanel Awaked");

            //TODO: Button.onClick

            Transform panelTransform = __instance.transform;

            Transform widgetTransform = Templates.AutoUploadWidget.transform;
            widgetTransform.SetParent(panelTransform, false);
            widgetTransform.position = new Vector3(0, -13, 7);

            //Templates.AutoUploadBackground.transform.SetParent(widgetTransform, false);
            BepinexPlugin.log.LogDebug("4");
            Templates.AutoUploadSwitch.transform.SetParent(widgetTransform, false);
            BepinexPlugin.log.LogDebug("5");
            Templates.AutoUploadText.transform.SetParent(widgetTransform, false);
            BepinexPlugin.log.LogDebug("6");
            Templates.AutoUploadTextArea.transform.SetParent(panelTransform, false);
            BepinexPlugin.log.LogDebug("7");
            Templates.AutoUploadEdit.transform.SetParent(widgetTransform, false);
            BepinexPlugin.log.LogDebug("8");
            Templates.AutoUploadUpload.transform.SetParent(widgetTransform, false);

            ////Canvas val5 = Object.Instantiate<Canvas>(tab, new InstantiateParameters
            ////{
            ////    parent = ((Component)tab).transform.parent
            ////});
            ////UiPanel
            //Image image = parent.AddComponent

            LayoutRebuilder.ForceRebuildLayoutImmediate(widgetTransform.GetComponent<RectTransform>());
            Templates.HasAppened = true;
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void OnShowingPatch(GameResultPanel __instance)
        {
            //BepinexPlugin.log.LogDebug("MainMenuPanel Awaked");
            BepinexPlugin.log.LogDebug("GameResultPanel OnShowing");
            GameResultPanel panel = __instance;
            //Templates.AutoUploadText.transform.SetParent(widgetTransform, false);
            //Templates.AutoUploadTextArea.transform.SetParent(panelTransform, false);


            //GameObject profileButton = panel.transform.Find("Main/Profile/ProfileNameBg").gameObject;
            //GameObject container = Object.Instantiate<GameObject>(profileButton, panel.transform);



            //TextMeshProUGUI test = Object.Instantiate<TextMeshProUGUI>(___profileNameText, new InstantiateParameters() { parent = ___profileNameText.transform.parent });
            //TextMeshProUGUI test = Object.Instantiate<TextMeshProUGUI>(___profileNameText, new Vector3(1.0f, 2.0f, 0.0f), Quaternion.identity);
            //test.text = "hoge";
            //test.transform.localPosition = new Vector3(test.transform.localPosition.x * 2, test.transform.localPosition.y, test.transform.localPosition.z * 2);
            //test.SetA

            ////{
            ////    parent = ((Component)tab).transform.parent
            ////});
            ////UiPanel
            //Image image = parent.AddComponent
        }
    }
}