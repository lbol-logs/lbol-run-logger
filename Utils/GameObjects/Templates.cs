using LBoL.Presentation.I10N;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        internal static GameObject Panel;
        internal static GameObject Control;
        internal static GameObject Switch;
        internal static GameObject Text;
        internal static GameObject Edit;
        internal static GameObject Upload;
        internal static GameObject TextArea;

        internal static GameObject Background;
        internal static GameObject PanelClone;

        internal static GameObject CreateGameObject(string name)
        {
            return new GameObject(name, typeof(RectTransform));
        }

        internal static GameObject CopyGameObject(Component parent, string name)
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
            LocalizedText localizedText = transform.GetComponent<LocalizedText>();
            localizedText.enabled = false;
            localizedText.key = null;
            //Object.Destroy(localizedText.gameObject);
            //Object.Destroy(transform.GetComponent<LocalizedText>().gameObject);
            transform.GetComponent<TextMeshProUGUI>().text = text;
        }

        internal static void CopyComponent<T>(GameObject dstGameObject, GameObject srcGameObject) where T : Component
        {
            T dst = dstGameObject.AddComponent<T>();
            T src = srcGameObject.GetComponent<T>();
            System.Type type = src.transform.GetComponent<T>().GetType();
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            //foreach (FieldInfo fieldInfo in fieldInfos) fieldInfo.SetValue(dst, fieldInfo.GetValue(src));
            BepinexPlugin.log.LogDebug("fields");
            foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
            {
                fieldInfo.SetValue(dst, fieldInfo.GetValue(src));
                BepinexPlugin.log.LogDebug($"name: {fieldInfo.Name}, src: {fieldInfo.GetValue(src)}, dst: {fieldInfo.GetValue(dst)}");
            }
            //BepinexPlugin.log.LogDebug("properties");
            //foreach (PropertyInfo propertyInfo in type.GetProperties(bindingFlags))
            //{
            //    if (!propertyInfo.CanRead || !propertyInfo.CanWrite || propertyInfo.Name == "name") continue;
            //    try
            //    {
            //        propertyInfo.SetValue(dst, propertyInfo.GetValue(src));
            //        BepinexPlugin.log.LogDebug($"name: {propertyInfo.Name}, src: {propertyInfo.GetValue(src)}, dst: {propertyInfo.GetValue(dst)}");
            //    }
            //    catch
            //    {
            //        BepinexPlugin.log.LogDebug($"> error: {propertyInfo.Name}");
            //    }
            //}
        }
    }
}