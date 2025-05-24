using LBoL.Presentation.I10N;
using LBoL.Presentation.UI;
using LBoL.Presentation.UI.ExtraWidgets;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace RunLogger.Utils
{
    internal static class ObjectsManager
    {
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
                    ObjectsManager.Object.QuickUpload,
                    ObjectsManager.Object.Status,
                    ObjectsManager.Object.TextArea
                };
                return gameObjects.Where(gameObject => gameObject != null);
            }
        }

        internal static Transform Initialize()
        {
            GameObject panel = ObjectsManager.Object.Panel;
            if (panel == null)
            {
                panel = ObjectsManager.Object.Panel = new GameObject("UploadPanel", typeof(RectTransform));
                RectTransform panelT = panel.GetComponent<RectTransform>();
                //panelT.anchorMin = new Vector2(0.5f, 0);
                //panelT.anchorMax = new Vector2(0.5f, 0);

                GameObject upload = ObjectsManager.Object.Upload = new GameObject("Upload", typeof(RectTransform));
                Transform uploadT = upload.transform;
                uploadT.SetParent(panelT, true);
            }
            return panel.transform;
        }

        internal static void ChangeText(Transform transform, string text)
        {
            LocalizedText localizedText = transform.GetComponent<LocalizedText>();
            localizedText.enabled = false;
            localizedText.key = null;
            transform.GetComponent<TextMeshProUGUI>().text = text;
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

        internal static void DestroyAllObjects()
        {
            foreach (GameObject gameObject in ObjectsManager.Objects) UnityEngine.Object.Destroy(gameObject);
        }
    }
}