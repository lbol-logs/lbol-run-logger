using HarmonyLib;
using LBoL.Core;
using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.ExtraWidgets;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using RunLogger.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class GameResultPanelPatch
    {
        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void DisplayPanel(GameResultPanel __instance)
        {
            Transform panelT = ObjectsManager.Initialize();
            panelT.SetParent(__instance.transform, true);

            GameObject autoUpload = ObjectsManager.Objects.AutoUpload = Object.Instantiate(
                UiManager.GetPanel<SettingPanel>().transform.Find("Root/Main/LeftPanel/AnimatingEnvironment").gameObject,
                new InstantiateParameters
                {
                    parent = panelT,
                    worldSpace = true
                    //worldSpace = false
                }
            );
            Transform autoUploadT = autoUpload.transform;
            autoUpload.name = "AutoUpload";
            autoUpload.GetComponent<CanvasGroup>().alpha = 1;
            autoUploadT.position = new Vector3(0, -14.8f, 10);
            int i = Helpers.CurrentSaveIndex;
            string title = $"Auto Upload Log #{i}";
            string description = StringDecorator.Decorate($"Auto upload the log of |Profile #{i}| to LBoL Logs.\nIf set to |false|, you can upload with description at the result screen.\nChange is effective from next run.\nUploaded log will be deleted from local drive.");
            SimpleTooltipSource.CreateDirect(autoUpload, title, description).WithPosition(TooltipDirection.Top, TooltipAlignment.Min);

            GameObject label = autoUploadT.Find("KeyTmp").gameObject;
            label.name = "Label";
            ObjectsManager.ChangeText(label, "Auto Upload");

            GameObject switchO = autoUploadT.Find("Switch").gameObject;
            RectTransform switchT = switchO.GetComponent<RectTransform>();
            switchT.localPosition = new Vector3(switchT.localPosition.x - 600, switchT.localPosition.y, switchT.localPosition.z);
            SwitchWidget switchWidget = switchO.GetComponent<SwitchWidget>();
            switchWidget.onToggleChanged = new UnityEvent<bool>();
            switchWidget.onToggleChanged.AddListener(value => Helpers.AutoUpload = value);
            switchWidget.SetValueWithoutNotifier(Helpers.AutoUpload, true);

            Transform bgT = ObjectsManager.Objects.Bg?.transform;
            if (bgT != null)
            {
                bgT.SetAsFirstSibling();
            }

            Transform textAreaT = ObjectsManager.Objects.TextArea?.transform;
            Transform editT = ObjectsManager.Objects.Edit?.transform;
            if (textAreaT != null && editT != null)
            {
                textAreaT.SetAsLastSibling();
            }

            //TODO: positioning
            //TODO: switch position, onchange, default value

            if (Helpers.AutoUpload) return;

            Transform uploadT = ObjectsManager.Objects.Upload?.transform;
            if (uploadT != null)
            {
                //TODO
            }
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnHiding)), HarmonyPostfix]
        private static void DestroyAllObjects()
        {
            ObjectsManager.DestroyAllObjects();
        }
    }
}