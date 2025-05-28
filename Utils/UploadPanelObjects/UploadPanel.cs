using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Utils.UploadPanelObjects
{
    internal static class UploadPanel
    {
        internal static bool HasPanel
        {
            get
            {
                Transform panel = ObjectsManager.Panel;
                if (panel != null) return true;
                if (ObjectsManager.PanelTemp == null)
                {
                    panel = new GameObject("UploadPanelTemp", typeof(RectTransform)).transform;
                    Transform upload = new GameObject("Upload", typeof(RectTransform)).transform;
                    upload.SetParent(panel, true);
                    upload.position = PositionsManager.UploadPosition;
                    upload.gameObject.SetActive(false);
                }
                return false;
            }
        }

        internal static void AdjustPanel()
        {
            if (ObjectsManager.PanelTemp.childCount != 7) return;

            Transform bg = ObjectsManager.GetFromTemp("Bg");
            Transform status = ObjectsManager.GetFromTemp("Status");
            Transform autoUpload = ObjectsManager.GetFromTemp("AutoUpload");
            Transform upload = ObjectsManager.GetFromTemp("Upload");
            Transform textArea = ObjectsManager.GetFromTemp("TextArea");

            Transform edit = upload.Find("Edit");
            Transform quickUpload = ObjectsManager.GetFromTemp("QuickUpload");
            Transform input = ObjectsManager.GetFromTemp("Input");

            quickUpload.SetParent(upload, false);
            input.SetParent(textArea, true);

            Image image = input.Find("TextFilterInput").GetComponent<Image>();
            image.sprite = bg.GetComponent<Image>().sprite;
            image.type = Image.Type.Sliced;

            Transform[] transforms = new[] { bg, status, autoUpload, upload, textArea };
            for (int i = 0; i < transforms.Length; i++) transforms[i].SetSiblingIndex(i);

            edit.SetAsLastSibling();

            ObjectsManager.PanelTemp.name = "UploadPanel";
        }
    }
}