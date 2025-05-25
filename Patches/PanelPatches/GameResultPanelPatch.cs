using HarmonyLib;
using LBoL.Core;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using RunLogger.Utils;
using RunLogger.Utils.UploadPanel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RunLogger.Patches.PanelPatches
{
    //[HarmonyPatch]
    internal static class GameResultPanelPatch
    {
        private const float X = 0;
        private const float Y = -14.88f;
        private const float BgOffsetX = -3.7f;
        private const float UploadOffsetX = 0.7f;
        private const float QuickUploadOffsetX = 0.5f;
        private const float QuickUploadScale = 0.004f;
        private const float StatusOffsetY = 0.06f;
        private static readonly Vector3 BgPosition = new Vector3(X + BgOffsetX, Y, 10);
        private static readonly Vector3 AutoUploadPosition = new Vector3(X, Y, 10);
        private static readonly Vector3 UploadPosition = new Vector3(X + UploadOffsetX, Y, 10);
        private static readonly Vector3 StatusPosition = new Vector3(X + UploadOffsetX, Y + StatusOffsetY, 10);

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void DisplayPanel(GameResultPanel __instance)
        {
            Transform panelT = ObjectsManager.Initialize();
            panelT.SetParent(__instance.transform, true);
BepinexPlugin.log.LogDebug(1);
            GameObject autoUpload = ObjectsManager.Object.AutoUpload = Object.Instantiate(
                UiManager.GetPanel<SettingPanel>().transform.Find("Root/Main/LeftPanel/AnimatingEnvironment").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                }
            );
            Transform autoUploadT = autoUpload.transform;
            autoUpload.name = "AutoUpload";
            autoUpload.GetComponent<CanvasGroup>().alpha = 1;
            autoUploadT.position = GameResultPanelPatch.AutoUploadPosition;
            int i = Helpers.CurrentSaveIndex;
            string title = $"Auto Upload Log #{i}";
            string description = StringDecorator.Decorate($"Auto upload the log of |Profile #{i}| to LBoL Logs.\nIf set to |false|, you can upload with description at the result screen.\nChange is effective from next run.\nUploaded log will be deleted from local drive.");
            ObjectsManager.SetTooltip(autoUpload, title, description);
BepinexPlugin.log.LogDebug(2);
            GameObject label = autoUploadT.Find("KeyTmp").gameObject;
            label.name = "Label";
            ObjectsManager.ChangeText(label.transform, "Auto Upload");
BepinexPlugin.log.LogDebug(3);
            GameObject switchO = autoUploadT.Find("Switch").gameObject;
            RectTransform switchT = switchO.GetComponent<RectTransform>();
            switchT.localPosition = new Vector3(switchT.localPosition.x - 600, switchT.localPosition.y, switchT.localPosition.z);
            SwitchWidget switchWidget = switchO.GetComponent<SwitchWidget>();
            switchWidget.onToggleChanged = new UnityEvent<bool>();
            switchWidget.onToggleChanged.AddListener(isOn => Helpers.AutoUpload = isOn);
            switchWidget.SetValueWithoutNotifier(Helpers.AutoUpload, true);
BepinexPlugin.log.LogDebug(4);
BepinexPlugin.log.LogDebug(ObjectsManager.Object.Status != null);
            Transform statusT = ObjectsManager.Object.Status.transform;
BepinexPlugin.log.LogDebug("A");
BepinexPlugin.log.LogDebug("B");
            statusT.position = GameResultPanelPatch.StatusPosition;
BepinexPlugin.log.LogDebug("C");
            statusT.SetAsFirstSibling();
BepinexPlugin.log.LogDebug("D");
BepinexPlugin.log.LogDebug(5);

            RectTransform bgT = ObjectsManager.Object.Bg.GetComponent<RectTransform>();
            bgT.position = GameResultPanelPatch.BgPosition;
            bgT.SetAsFirstSibling();
BepinexPlugin.log.LogDebug(6);

            Transform textAreaT = ObjectsManager.Object.TextArea.transform;
            Transform editT = ObjectsManager.Object.Edit.transform;
            Transform inputT = ObjectsManager.Object.Input.transform;
            editT.SetAsLastSibling();
            textAreaT.SetAsLastSibling();

            inputT.SetParent(textAreaT, true);

            Image image = inputT.Find("TextFilterInput").GetComponent<Image>();
            image.sprite = bgT.GetComponent<Image>().sprite;
            image.type = Image.Type.Sliced;
BepinexPlugin.log.LogDebug(7);

            if (!Helpers.AutoUpload)
            {
                RectTransform uploadT = ObjectsManager.Object.Upload.GetComponent<RectTransform>();
                uploadT.position = GameResultPanelPatch.UploadPosition;

                GameObject quickUpload = ObjectsManager.Object.QuickUpload = Object.Instantiate(
                    __instance.transform.Find("CommonButton").gameObject,
                    new InstantiateParameters
                    {
                        parent = uploadT,
                        worldSpace = true
                    }
                );
                quickUpload.name = "QuickUpload";
                RectTransform quickUploadT = quickUpload.GetComponent<RectTransform>();
                ObjectsManager.SetClickEvent(quickUploadT, () => LBoLLogs.Upload());
                ObjectsManager.ChangeText(quickUploadT.Find("Layout/Text (TMP)"), "Upload");
                quickUploadT.pivot = new Vector2(0, 0.5f);
                quickUploadT.localPosition = new Vector2(GameResultPanelPatch.QuickUploadOffsetX, 0);
                quickUploadT.localScale = new Vector3(GameResultPanelPatch.QuickUploadScale, GameResultPanelPatch.QuickUploadScale, GameResultPanelPatch.QuickUploadScale);
            }
BepinexPlugin.log.LogDebug(8);
            foreach (GameObject gameObject in ObjectsManager.Objects)
            {
                if (gameObject == ObjectsManager.Object.TextArea) continue;
                if (gameObject == ObjectsManager.Object.Status) continue;
                if (Helpers.AutoUpload && gameObject == ObjectsManager.Object.Upload) continue;
                gameObject.SetActive(true);
            }
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnHiding)), HarmonyPostfix]
        private static void DestroyAllObjects()
        {
            ObjectsManager.DestroyAllObjects();
        }
    }
}