using LBoL.Core;
using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.ExtraWidgets;
using System;
using System.Collections.Generic;
using System.Linq;
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
        internal static RectTransform Panel
        {
            get
            {
                Scene scene = SceneManager.GetSceneByName("GameRun");
                GameObject[] gameObjects = scene.GetRootGameObjects();
                return Array.Find(gameObjects, gameObject => gameObject.name == "UploadPanel")?.GetComponent<RectTransform>();
            }
        }

        private static GameObject Clone
        {
            get
            {
                return UiManager.GetPanel<GameResultPanel>().transform.Find("UploadPanel (Clone)")?.gameObject;
            }
        }

        internal static class Object
        {
            internal static GameObject Panel;
            internal static GameObject Bg;
            internal static GameObject AutoUpload;
            internal static GameObject Upload;
            internal static GameObject Edit;
            internal static GameObject QuickUpload;
            internal static GameObject Status;
            internal static GameObject TextArea;
            internal static GameObject Input;
        }

        internal static IEnumerable<GameObject> Objects
        {
            get
            {
                GameObject[] gameObjects = new[]
                {
                    ObjectsManager.Object.Panel,
                    ObjectsManager.Object.Bg,
                    ObjectsManager.Object.AutoUpload,
                    ObjectsManager.Object.Upload,
                    ObjectsManager.Object.Edit,
                    ObjectsManager.Object.Upload,
                    ObjectsManager.Object.QuickUpload,
                    ObjectsManager.Object.Status,
                    ObjectsManager.Object.TextArea,
                    ObjectsManager.Object.Input
    };
                return gameObjects.Where(gameObject => gameObject != null);
            }
        }

        internal static string Text
        {
            get
            {
                TMP_InputField tmpInput = ObjectsManager.TmpInput;
                return tmpInput.text;
            }
            set
            {
                TMP_InputField tmpInput = ObjectsManager.TmpInput;
                tmpInput.text = value;
            }
        }

        internal static TMP_InputField TmpInput
        {
            get
            {
                Transform inputT = ObjectsManager.Object.Input.transform;
                TMP_InputField tmpInput = inputT.Find("TextFilterInput").GetComponent<TMP_InputField>();
                return tmpInput;
            }
        }

        internal static Transform Initialize()
        {
            BepinexPlugin.log.LogDebug(ObjectsManager.Panel != null);
            GameObject panel = ObjectsManager.Object.Panel;
            if (panel == null)
            {
                panel = ObjectsManager.Object.Panel = new GameObject("UploadPanel", typeof(RectTransform));
                RectTransform panelT = panel.GetComponent<RectTransform>();

                GameObject upload = ObjectsManager.Object.Upload = new GameObject("Upload", typeof(RectTransform));
                Transform uploadT = upload.transform;
                uploadT.SetParent(panelT, true);
            }
            return panel.transform;
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

        internal static void SetTooltip(GameObject gameObject, string title, string description = null)
        {
            SimpleTooltipSource.CreateDirect(gameObject, title, description).WithPosition(TooltipDirection.Top, TooltipAlignment.Min);
        }


        internal static void UpdateStatus(string uploadStatus, string url)
        {
            Transform statusT = ObjectsManager.Object.Status.transform;
            statusT.gameObject.SetActive(true);
            string text = url == null ? uploadStatus : $"<u>{uploadStatus}</u>";
            Transform textT = statusT.Find("SeedText");
            string color = url == null ? null : ColorUtility.ToHtmlStringRGBA(GlobalConfig.UiBlue);
            ObjectsManager.ChangeText(textT, text, color);
            if (url == null) return;
            ObjectsManager.SetClickEvent(statusT, () => Application.OpenURL(url));
        }

        internal static void DestroyClone()
        {
            GameObject clone = ObjectsManager.Clone;
            if (clone != null) UnityEngine.Object.Destroy(clone);
        }
    }
}