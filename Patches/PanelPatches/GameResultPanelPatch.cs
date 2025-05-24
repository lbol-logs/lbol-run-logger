using HarmonyLib;
using LBoL.Core;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using RunLogger.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class GameResultPanelPatch
    {
        private const float X = 0;
        private const float Y = -14.88f;
        private const float UploadOffsetX = 0.7f;
        private const float StatusOffsetY = 0.06f;
        private static readonly Vector3 AutoUploadPosition = new Vector3(X, Y, 10);
        private static readonly Vector3 UploadPosition = new Vector3(X + UploadOffsetX, Y, 10);
        private static readonly Vector3 StatusPosition = new Vector3(X + UploadOffsetX, Y + StatusOffsetY, 10);

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void DisplayPanel(GameResultPanel __instance)
        {
            Transform panelT = ObjectsManager.Initialize();
            panelT.SetParent(__instance.transform, true);

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

            GameObject label = autoUploadT.Find("KeyTmp").gameObject;
            label.name = "Label";
            ObjectsManager.ChangeText(label.transform, "Auto Upload");

            GameObject switchO = autoUploadT.Find("Switch").gameObject;
            RectTransform switchT = switchO.GetComponent<RectTransform>();
            switchT.localPosition = new Vector3(switchT.localPosition.x - 600, switchT.localPosition.y, switchT.localPosition.z);
            SwitchWidget switchWidget = switchO.GetComponent<SwitchWidget>();
            switchWidget.onToggleChanged = new UnityEvent<bool>();
            switchWidget.onToggleChanged.AddListener(isOn => Helpers.AutoUpload = isOn);
            switchWidget.SetValueWithoutNotifier(Helpers.AutoUpload, true);

            Transform statusT = ObjectsManager.Object.Status?.transform;
            if (statusT != null)
            {
                statusT.position = GameResultPanelPatch.StatusPosition;
                statusT.SetAsFirstSibling();
            }

            Transform bgT = ObjectsManager.Object.Bg?.transform;
            if (bgT != null)
            {
                bgT.SetAsFirstSibling();
            }

            Transform textAreaT = ObjectsManager.Object.TextArea?.transform;
            Transform editT = ObjectsManager.Object.Edit?.transform;
            if (textAreaT != null && editT != null)
            {
                editT.SetAsLastSibling();
                textAreaT.SetAsLastSibling();
            }

            //TODO: positioning
            //TODO: switch position, onchange, default value

            if (!Helpers.AutoUpload)
            {
                RectTransform uploadT = ObjectsManager.Object.Upload.GetComponent<RectTransform>();
                uploadT.position = GameResultPanelPatch.UploadPosition;
            }

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