using LBoL.Presentation.I10N;
using TMPro;
using UnityEngine;

namespace RunLogger.Utils
{
    internal static class ObjectsManager
    {
        internal static class Objects
        {
            internal static GameObject Panel;
            internal static GameObject Bg;
            internal static GameObject AutoUpload;
            internal static GameObject Upload;
            internal static GameObject QuickUpload;
            internal static GameObject Status;
            internal static GameObject TextArea;
        }

        internal static Transform Initialize()
        {
            GameObject panel = ObjectsManager.Objects.Panel;
            if (panel == null)
            {
                panel = ObjectsManager.Objects.Panel = new GameObject("UploadPanel", typeof(RectTransform));
                RectTransform panelT = panel.GetComponent<RectTransform>();
                //panelT.anchorMin = new Vector2(0.5f, 0);
                //panelT.anchorMax = new Vector2(0.5f, 0);

                GameObject upload = ObjectsManager.Objects.Upload = new GameObject("Upload", typeof(RectTransform));
                Transform uploadT = upload.transform;
                uploadT.SetParent(panelT, true);
            }
            return panel.transform;
        }

        internal static void ChangeText(GameObject gameObject, string text)
        {
            LocalizedText localizedText = gameObject.GetComponent<LocalizedText>();
            localizedText.enabled = false;
            localizedText.key = null;
            gameObject.GetComponent<TextMeshProUGUI>().text = text;
        }

        internal static void DestroyAllObjects()
        {
            GameObject[] gameObjects = new[]
            {
                ObjectsManager.Objects.Panel,
                ObjectsManager.Objects.Bg,
                ObjectsManager.Objects.AutoUpload,
                ObjectsManager.Objects.Upload,
                ObjectsManager.Objects.QuickUpload,
                ObjectsManager.Objects.Status,
                ObjectsManager.Objects.TextArea
            };
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject != null) Object.Destroy(gameObject);
            }
        }
    }
}