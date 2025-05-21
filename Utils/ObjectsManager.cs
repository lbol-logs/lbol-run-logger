using UnityEngine;

namespace RunLogger.Utils
{
    internal static class ObjectsManager
    {
        internal static class Objects
        {
            internal static GameObject Panel;
            internal static GameObject AutoUpload;
            internal static GameObject Upload;
            internal static GameObject Status;
            internal static GameObject TextArea;
        }

        internal static void Initialize()
        {
            if (ObjectsManager.Objects.Panel != null) return;

            GameObject panel = ObjectsManager.Objects.Panel = new GameObject("UploadPanel", typeof(RectTransform));
            RectTransform panelTransform = panel.GetComponent<RectTransform>();
            panelTransform.anchorMin = new Vector2(0.5f, 0);
            panelTransform.anchorMax = new Vector2(0.5f, 0);

            GameObject upload = ObjectsManager.Objects.Upload = new GameObject("Upload", typeof(RectTransform));
            Transform uploadTransform = upload.transform;
            uploadTransform.SetParent(panelTransform, true);
        }

        internal static void DestroyAllObjects()
        {
            GameObject[] gameObjects = new[]
            {
                ObjectsManager.Objects.Panel,
                ObjectsManager.Objects.AutoUpload,
                ObjectsManager.Objects.Upload,
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