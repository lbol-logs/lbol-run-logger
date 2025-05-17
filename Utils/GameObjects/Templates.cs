using LBoL.Presentation.I10N;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace RunLogger.Utils.GameObjects
{
    internal static class Templates
    {
        internal static class Names
        {
            internal const string Panel = "AutoUploadPanel";
            internal const string Control = "Control";
            internal const string Switch = "Switch";
            internal const string Text = "Text";
            internal const string Edit = "Edit";
            internal const string Upload = "Upload";
            internal const string TextArea = "TextArea";
        }

        internal static GameObject AutoUploadPanel;
        internal static GameObject AutoUploadControl;
        internal static GameObject AutoUploadSwitch;
        internal static GameObject AutoUploadText;
        internal static GameObject AutoUploadTextArea;
        internal static GameObject AutoUploadEdit;
        internal static GameObject AutoUploadUpload;

        internal static GameObject Background;
        internal static GameObject AutoUploadPanelClone;

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
            Object.Destroy(transform.GetComponent<LocalizedText>().gameObject);
            transform.GetComponent<TextMeshProUGUI>().text = text;
        }
    }
}