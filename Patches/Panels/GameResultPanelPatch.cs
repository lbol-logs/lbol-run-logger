using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using System.Reflection;
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
            BepinexPlugin.log.LogDebug("GameResultPanel Awaked");

            Transform panelTransform = __instance.transform;

            //TODO: Button.onClick
            if (Templates.AutoUploadPanel == null)
            {
                GameObject autoUploadPanel = Object.Instantiate(Templates.AutoUploadTextArea);
                BepinexPlugin.log.LogDebug("A");
                autoUploadPanel.name = Templates.Names.Panel;
                BepinexPlugin.log.LogDebug("B");
                Object.Destroy(autoUploadPanel.transform.Find("NameInput").gameObject);
                BepinexPlugin.log.LogDebug("C");
                Object.Destroy(autoUploadPanel.GetComponent<Image>().gameObject);
                BepinexPlugin.log.LogDebug("D");
                Object.Destroy(autoUploadPanel.GetComponent<CanvasRenderer>());
                BepinexPlugin.log.LogDebug("E");
                //autoUploadPanel.transform.position = new Vector3(0, 0, 0);

                GameObject autoUploadControl = Templates.AutoUploadControl;
                Transform controlTransform = autoUploadControl.transform;

                //Object.Destroy(controlTransform.GetComponent<Image>().gameObject);
                //autoUploadControl.AddComponent<Image>();
                BepinexPlugin.log.LogDebug("1");
                //controlTransform.GetComponent<Image>().sprite = Templates.Background.GetComponent<Image>().sprite;
                BepinexPlugin.log.LogDebug("2");
                GameObject background = Templates.Background;
                FieldInfo[] fieldInfos = background.transform.GetComponent<Image>().GetType().GetFields();
                foreach (FieldInfo fieldInfo in fieldInfos) fieldInfo.SetValue(autoUploadControl, fieldInfo.GetValue(background));
                BepinexPlugin.log.LogDebug("B");

                controlTransform.position = new Vector3(0, -12.4f, 5);
                BepinexPlugin.log.LogDebug("3");
                Templates.AutoUploadSwitch.transform.SetParent(controlTransform, false);
                LayoutElement layoutElement = Templates.AutoUploadSwitch.AddComponent<LayoutElement>();
                layoutElement.flexibleWidth = 1;
                layoutElement.preferredWidth = 200;
                BepinexPlugin.log.LogDebug("4");
                Templates.AutoUploadText.transform.SetParent(controlTransform, false);
                Templates.AutoUploadEdit.transform.SetParent(controlTransform, false);
                Templates.AutoUploadUpload.transform.SetParent(controlTransform, false);
                BepinexPlugin.log.LogDebug("5");
                GridLayoutGroup gridLayoutGroup = controlTransform.GetComponent<GridLayoutGroup>();
                gridLayoutGroup.CalculateLayoutInputHorizontal();
                gridLayoutGroup.CalculateLayoutInputVertical();
                gridLayoutGroup.SetLayoutHorizontal();
                gridLayoutGroup.SetLayoutVertical();
                BepinexPlugin.log.LogDebug("6");
                autoUploadControl.transform.SetParent(autoUploadPanel.transform, true);
                BepinexPlugin.log.LogDebug("7");
                Templates.AutoUploadTextArea.transform.SetParent(autoUploadPanel.transform, false);

                ////Canvas val5 = Object.Instantiate<Canvas>(tab, new InstantiateParameters
                ////{
                ////    parent = ((Component)tab).transform.parent
                ////});
                ////UiPanel
                //Image image = parent.AddComponent
                Templates.AutoUploadPanel = autoUploadPanel;
            }

            if (panelTransform.Find(Templates.Names.Panel) != null) return;
            BepinexPlugin.log.LogDebug("8");
            GameObject gameObject = Templates.AutoUploadPanelClone = Object.Instantiate(Templates.AutoUploadPanel);
            BepinexPlugin.log.LogDebug("9");
            gameObject.transform.SetParent(panelTransform, false);
            BepinexPlugin.log.LogDebug("10");
            gameObject.SetActive(true);
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void OnShowingPatch(GameResultPanel __instance)
        {
            //BepinexPlugin.log.LogDebug("MainMenuPanel Awaked");
            BepinexPlugin.log.LogDebug("GameResultPanel OnShowing");
            GameResultPanel panel = __instance;
            //Templates.AutoUploadText.transform.SetParent(controlTransform, false);
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