using LBoL.Presentation.I10N;
using TMPro;
using UnityEngine;

namespace RunLogger.Utils.GameObjects
{
    internal static class Templates
    {
        internal static class Names
        {
            internal const string Widget = "AutoUploadWidget";
            internal const string Switch = "Switch";
            internal const string Text = "Text";
            internal const string TextArea = "TextArea";
            internal const string Edit = "Edit";
            internal const string Upload = "Upload";
        }

        internal static GameObject AutoUploadWidget;
        internal static GameObject AutoUploadSwitch;
        internal static GameObject AutoUploadText;
        internal static GameObject AutoUploadTextArea;
        internal static GameObject AutoUploadEdit;
        internal static GameObject AutoUploadUpload;

        internal static bool HasAppened;

        internal static GameObject Create(Component parent, string name)
        {
            GameObject original = parent.transform.Find(name).gameObject;
            return Object.Instantiate(original);
        }

        internal static void RemoveChildren(GameObject gameObject, string[] names)
        {
            foreach (string name in names) Object.Destroy(gameObject.transform.Find(name).gameObject);
        }

        internal static void ChangeText(GameObject gameObject, string name, string text)
        {
            Transform transform = gameObject.transform.Find(name);
            Object.Destroy(transform.GetComponent<LocalizedText>());
            transform.GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}