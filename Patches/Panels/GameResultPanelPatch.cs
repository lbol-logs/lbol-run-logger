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

            Transform gameResultPanelTransform = __instance.transform;

            //TODO: Button.onClick
            if (Templates.Panel == null)
            {
                GameObject panel = Templates.CreateGameObject(Templates.Names.Panel);
                Transform panelTransform = panel.transform;
                {
                    panel.SetActive(false);
                    RectTransform transform = panel.GetComponent<RectTransform>();
                    //transform.localPosition = Vector3.zero;
                    //transform.localScale = Vector3.one;
                    transform.anchorMin = Vector2.zero;
                    transform.anchorMax = Vector2.one;
                }

                //BepinexPlugin.log.LogDebug($"TextArea: {Templates.TextArea != null}");
                //GameObject panel = Object.Instantiate(Templates.TextArea);
                //BepinexPlugin.log.LogDebug("A");
                //panel.name = Templates.Names.Panel;
                //BepinexPlugin.log.LogDebug("B");
                //Object.Destroy(panel.transform.Find("NameInput").panelClone);
                //BepinexPlugin.log.LogDebug("C");
                //Object.Destroy(panel.GetComponent<Image>().panelClone);
                //BepinexPlugin.log.LogDebug("D");
                //Object.Destroy(panel.GetComponent<CanvasRenderer>());
                //BepinexPlugin.log.LogDebug("E");
                //panel.transform.position = new Vector3(0, 0, 0);

                GameObject control = Templates.Control;
                Transform controlTransform = control.transform;
                {
                    RectTransform transform = control.GetComponent<RectTransform>();
                    //transform.localPosition = Vector3.zero;
                    //transform.localScale = Vector3.one;
                    transform.anchorMin = new Vector2(0.2f, 0);
                    transform.anchorMax = new Vector2(0.8f, 0.2f);
                }

                //Object.Destroy(controlTransform.GetComponent<Image>().panelClone);
                //controlTransform.GetComponent<Image>().sprite = Templates.Background.GetComponent<Image>().sprite;

                Templates.CopyComponent<Image>(control, Templates.Background);
                Templates.CopyComponent<CanvasGroup>(control, Templates.TextArea);

                //GameObject background = Templates.Background;
                //FieldInfo[] fieldInfos = background.transform.GetComponent<Image>().GetType().GetFields();
                //foreach (FieldInfo fieldInfo in fieldInfos) fieldInfo.SetValue(control, fieldInfo.GetValue(background));

                //controlTransform.position = new Vector3(0, -12.4f, 5);
                {
                    RectTransform transform = Templates.Switch.GetComponent<RectTransform>();
                    transform.SetParent(controlTransform, true);
                    //transform.localPosition = Vector3.zero;
                    //transform.anchorMin = new Vector2(0.7f, 0.5f);
                    //transform.anchorMax = new Vector2(0.7f, 0.5f);
                    //transform.offsetMin = new Vector2(-320, -60);
                    //transform.offsetMax = new Vector2(0, 60);
                }

                //LayoutElement layoutElement = Templates.Switch.AddComponent<LayoutElement>();
                //layoutElement.flexibleWidth = 1;
                //layoutElement.preferredWidth = 200;

                {
                    RectTransform transform = Templates.Text.GetComponent<RectTransform>();
                    transform.SetParent(controlTransform, true);
                    //transform.anchorMin = Vector2.zero;
                    //transform.anchorMax = Vector2.one;
                    //transform.offsetMin = new Vector2(10, 6);
                    //transform.offsetMax = new Vector2(-10, -7);
                }


                Templates.Edit.transform.SetParent(controlTransform, true);
                Templates.Upload.transform.SetParent(controlTransform, true);

                //GridLayoutGroup gridLayoutGroup = controlTransform.GetComponent<GridLayoutGroup>();
                //gridLayoutGroup.CalculateLayoutInputHorizontal();
                //gridLayoutGroup.CalculateLayoutInputVertical();
                //gridLayoutGroup.SetLayoutHorizontal();
                //gridLayoutGroup.SetLayoutVertical();

                control.transform.SetParent(panelTransform, true);
                Templates.TextArea.transform.SetParent(panelTransform, true);

                ////Canvas val5 = Object.Instantiate<Canvas>(tab, new InstantiateParameters
                ////{
                ////    parent = ((Component)tab).transform.parent
                ////});
                ////UiPanel
                //Image image = parent.AddComponent
                Templates.Panel = panel;
            }

            if (gameResultPanelTransform.Find(Templates.Names.Panel) != null) return;
            GameObject panelClone = Templates.PanelClone = Object.Instantiate(Templates.Panel);
            panelClone.transform.SetParent(gameResultPanelTransform, true);

            {
                RectTransform transform = panelClone.GetComponent<RectTransform>();
                //transform.localPosition = Vector3.zero;
                transform.anchorMin = new Vector2(-50, -50);
                transform.anchorMax = new Vector2(50, 50);
            }

            {
                RectTransform transform = Templates.Switch.GetComponent<RectTransform>();
                transform.localPosition = Vector3.zero;
                transform.anchorMin = new Vector2(0.7f, 0.5f);
                transform.anchorMax = new Vector2(0.7f, 0.5f);
                transform.offsetMin = new Vector2(-320, -60);
                transform.offsetMax = new Vector2(0, 60);
            }

            {
                RectTransform transform = Templates.Text.GetComponent<RectTransform>();
                transform.anchorMin = Vector2.zero;
                transform.anchorMax = Vector2.one;
                transform.offsetMin = new Vector2(10, 6);
                transform.offsetMax = new Vector2(-10, -7);
            }

            panelClone.SetActive(true);
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void OnShowingPatch(GameResultPanel __instance)
        {
            //BepinexPlugin.log.LogDebug("MainMenuPanel Awaked");
            BepinexPlugin.log.LogDebug("GameResultPanel OnShowing");
            GameResultPanel panel = __instance;
            //Templates.Text.transform.SetParent(controlTransform, false);
            //Templates.TextArea.transform.SetParent(gameResultPanelTransform, false);


            //GameObject profileButton = panel.transform.Find("Main/Profile/ProfileNameBg").panelClone;
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