using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Transitions;
using RunLogger.Utils.GameObjects;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class ProfilePanelPatch
    {
        [HarmonyPatch(typeof(ProfilePanel), nameof(ProfilePanel.Awake)), HarmonyPostfix]
        private static void CreateComponents(ProfilePanel __instance)
        {
            if (Templates.Edit != null && Templates.Upload != null && Templates.TextArea != null) return;
            ProfilePanel panel = __instance;
            if (Templates.Edit == null)
            {
                GameObject clone = Templates.CopyGameObject(panel, "Profiles/Layout/ProfileWidget0/Content/EditButton");
                clone.name = Templates.Names.Edit;
                Templates.Edit = clone;
            }
            if (Templates.Upload == null)
            {
                GameObject clone = Templates.CopyGameObject(panel, "NameInput/Confirm");
                clone.name = Templates.Names.Upload;
                Templates.ChangeText(clone, "Layout/Text (TMP)", "Upload");
                Templates.Upload = clone;

                GameObject control = Templates.CopyGameObject(clone.transform, "Layout");
                control.name = Templates.Names.Control;
                foreach (Transform child in control.transform) Object.Destroy(child.gameObject);
                Templates.Control = control;
            }
            if (Templates.TextArea == null)
            {
                //GameObject textArea = Templates.CreateGameObject(Templates.Names.TextArea);
                //textArea.SetActive(false);
                //Templates.CopyImageComponent(textArea, panel.gameObject);
                ////textArea.GetComponent<RectTransform>().anchorMax = Vector2.one;
                ////textArea.GetComponent<RectTransform>().anchorMin = Vector2.one;
                ////textArea.transform.localPosition = Vector3.zero;

                //GameObject clone = Templates.CopyGameObject(panel, "NameInput");
                //clone.transform.SetParent(textArea.transform, false);
                //clone.name = "NameInput";
                //Templates.RemoveChildren(clone, new[] { "Bg", "Title" });
                //Templates.TextArea = textArea;

                GameObject clone = Templates.CopyGameObject(panel, "NameInput");
                clone.SetActive(false);
                clone.name = Templates.Names.TextArea;
                Templates.RemoveChildren(clone, new[] { "Bg", "Title" });
                Templates.TextArea = clone;
            }
        }
    }
}