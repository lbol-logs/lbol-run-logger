using LBoL.Core;
using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.ExtraWidgets;
using LBoL.Presentation.UI.Panels;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace RunLogger.Utils.UploadPanelObjects
{
    internal static class ObjectsManager
    {
        internal static Transform Panel
        {
            get
            {
                return ObjectsManager.GetTransformFromGameRunScene("UploadPanel");
            }
        }

        internal static Transform PanelTemp
        {
            get
            {
                return ObjectsManager.GetTransformFromGameRunScene("UploadPanelTemp");
            }
        }

        private static Transform GetTransformFromGameRunScene(string name)
        {
            Scene scene = SceneManager.GetSceneByName("GameRun");
            GameObject[] gameObjects = scene.GetRootGameObjects();
            return Array.Find(gameObjects, gameObject => gameObject.name == name)?.transform;
        }

        internal static Transform Clone
        {
            get
            {
                if (!UiManager.Instance._panelTable.TryGetValue(typeof(GameResultPanel), out UiPanelBase uiPanelBase)) return null;
                return uiPanelBase.transform.Find("UploadPanel(Clone)");
            }
        }

        private static TMP_InputField TmpInput
        {
            get
            {
                return ObjectsManager.Clone.Find("TextArea/Input/TextFilterInput").GetComponent<TMP_InputField>();
            }
        }

        internal static string Text
        {
            get
            {
                return ObjectsManager.TmpInput.text;
            }
            set
            {
                ObjectsManager.TmpInput.text = value;
            }
        }

        internal static Transform GetFromTemp(string path)
        {
            return ObjectsManager.PanelTemp?.Find(path);
        }

        internal static RectTransform CopyGameObject(Transform transform, string path, Transform parent = null)
        {
            return UnityEngine.Object.Instantiate(
                transform.Find(path).gameObject,
                parent ?? ObjectsManager.PanelTemp,
                true
            ).GetComponent<RectTransform>();
        }

        internal static void ChangeText(Transform transform, string text, string color = null)
        {
            LocalizedText localizedText = transform.GetComponent<LocalizedText>();
            localizedText.enabled = false;
            localizedText.key = null;
            transform.GetComponent<TextMeshProUGUI>().text = color == null ? text : $"<color=#{color}>{text}</color>";
        }

        internal static void SetClickEvent(Transform transform, UnityAction call)
        {
            Button button = transform.GetComponent<Button>();
            button.onClick = new ButtonClickedEvent();
            button.onClick.AddListener(new UnityAction(call));
        }

        internal static void SetTooltip(Transform transform, string title, string description = null)
        {
            SimpleTooltipSource.CreateDirect(transform.gameObject, title, description).WithPosition(TooltipDirection.Top, TooltipAlignment.Min);
        }

        internal static void UpdateStatus(string uploadStatus, string url)
        {
            Transform status = ObjectsManager.Clone.Find("Status");
            string text = url == null ? uploadStatus : $"<u>{uploadStatus}</u>";
            Transform textT = status.Find("SeedText");
            string color = url == null ? null : ColorUtility.ToHtmlStringRGBA(GlobalConfig.UiBlue);
            ObjectsManager.ChangeText(textT, text, color);
            if (url == null) return;
            ObjectsManager.SetClickEvent(status, () => Application.OpenURL(url));
        }

        internal static void DestroyClone()
        {
            Transform clone = ObjectsManager.Clone;
            if (clone != null) UnityEngine.Object.Destroy(clone.gameObject);
        }
    }
}