using HarmonyLib;
using LBoL.Core;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Widgets;
using RunLogger.Utils;
using RunLogger.Utils.Enums;
using RunLogger.Utils.UploadPanelObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TMPro.TMP_InputField;

namespace RunLogger.Patches.PanelPatches
{
    [HarmonyPatch]
    internal static class GameResultPanelPatch
    {
        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.Awake)), HarmonyPostfix]
        private static void CreateQuickUpload(GameResultPanel __instance)
        {
            if (UploadPanel.HasPanel || ObjectsManager.GetFromTemp("QuickUpload") != null) return;

            RectTransform quickUpload = ObjectsManager.CopyGameObject(__instance.transform, "CommonButton");
            quickUpload.name = "QuickUpload";
            quickUpload.gameObject.SetActive(true);
            ObjectsManager.ChangeText(quickUpload.Find("Layout/Text (TMP)"), "Upload");
            quickUpload.pivot = new Vector2(0, 0.5f);
            quickUpload.localPosition = new Vector2(PositionsManager.QuickUploadOffsetX, 0);
            quickUpload.localScale = new Vector3(PositionsManager.QuickUploadScale, PositionsManager.QuickUploadScale, PositionsManager.QuickUploadScale);
            UploadPanel.AdjustPanel();
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnShowing)), HarmonyPostfix]
        private static void DisplayPanel(GameResultPanel __instance)
        {
            Transform panelTemplate = ObjectsManager.Panel;
            if (panelTemplate == null)
            {
                BepinexPlugin.log.LogWarning("UploadPanel is not ready");
                return;
            }
            Transform panel = Object.Instantiate(panelTemplate, __instance.transform, true);

            SwitchWidget switchWidget = panel.Find("AutoUpload/Switch").GetComponent<SwitchWidget>();
            switchWidget.onToggleChanged = new UnityEvent<bool>();
            switchWidget.onToggleChanged.AddListener(isOn => Helpers.AutoUpload = isOn);
            switchWidget.SetValueWithoutNotifier(Helpers.AutoUpload, true);

            int i = Helpers.CurrentSaveIndex;
            string title = $"Auto Upload Log #{i}";
            string description = StringDecorator.Decorate($"Auto upload the log of |Profile #{i}| to LBoL Logs.\nIf set to |false|, you can upload with description at the result screen.\nChange is effective from next run.\nUploaded log will be deleted from local drive.\nAbandoned run is never auto uploaded.");
            ObjectsManager.SetTooltip(panel.Find("AutoUpload"), title, description);

            if (Controller.Instance?.Path == null)
            {
                ObjectsManager.ChangeText(panel.Find("Status").Find("SeedText"), UploadStatus.NotSaved);
                return;
            }

            if (Helpers.AutoUpload && !Controller.Instance.IsAbandoned)
            {
                LBoLLogs.Upload();
                return;
            }

            panel.Find("Upload").gameObject.SetActive(true);

            Transform edit = panel.Find("Upload/Edit");
            Transform textArea = panel.Find("TextArea");

            TMP_InputField tmpInput = textArea.Find("Input/TextFilterInput").GetComponent<TMP_InputField>();
            TextMeshProUGUI count = textArea.Find("Count").GetComponent<TextMeshProUGUI>();
            tmpInput.onValueChanged = new OnChangeEvent();
            tmpInput.onValueChanged.AddListener(new UnityAction<string>(value => count.text = $"{value.Length}/300"));

            ObjectsManager.SetTooltip(edit, "Add description", "optional");
            ObjectsManager.SetClickEvent(edit, () =>
            {
                textArea.gameObject.SetActive(true);
            });

            ObjectsManager.SetClickEvent(panel.Find("Upload/QuickUpload"), () => LBoLLogs.Upload());

            ObjectsManager.SetClickEvent(textArea.Find("Confirm"), () =>
            {
                textArea.gameObject.SetActive(false);
                LBoLLogs.Upload(ObjectsManager.Text);
            });
            ObjectsManager.SetClickEvent(textArea.Find("Cancel"), () =>
            {
                textArea.gameObject.SetActive(false);
                ObjectsManager.Text = null;
            });
        }

        [HarmonyPatch(typeof(GameResultPanel), nameof(GameResultPanel.OnHiding)), HarmonyPostfix]
        private static void DestroyClone()
        {
            ObjectsManager.DestroyClone();
        }
    }
}