using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI.Transitions;
using RunLogger.Utils.GameObjects;
using UnityEngine;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class ProfilePanelPatch
    {
        private static bool IsAwaked;

        [HarmonyPatch(typeof(ProfilePanel), nameof(ProfilePanel.Awake)), HarmonyPostfix]
        private static void CreateComponents(ProfilePanel __instance)
        {
            if (Templates.AutoUploadEdit != null && Templates.AutoUploadUpload != null && Templates.AutoUploadTextArea != null) return;
            BepinexPlugin.log.LogDebug("ProfilePanel Awaked");
            ProfilePanel panel = __instance;
            if (Templates.AutoUploadEdit == null)
            {
                GameObject clone = Templates.Create(panel, "Profiles/Layout/ProfileWidget0/Content/EditButton");
                clone.name = Templates.Names.Edit;
                Templates.AutoUploadEdit = clone;
            }
            if (Templates.AutoUploadUpload == null)
            {
                GameObject clone = Templates.Create(panel, "NameInput/Confirm");
                clone.name = Templates.Names.Upload;
                Templates.ChangeText(clone, "Layout/Text (TMP)", "Upload");
                Templates.AutoUploadUpload = clone;
            }
            if (Templates.AutoUploadTextArea == null)
            {
                if (IsAwaked) return;
                ProfilePanelPatch.IsAwaked = true;
                GameObject clone = Object.Instantiate(panel.gameObject);
                clone.SetActive(false);
                Object.Destroy(clone.GetComponent<CanvasGroup>().gameObject);
                Object.Destroy(clone.GetComponent<ProfilePanel>().gameObject);
                Object.Destroy(clone.GetComponent<ProfileTransition>().gameObject);
                clone.name = Templates.Names.TextArea;
                Templates.RemoveChildren(clone, new[] { "Profiles", "DeleteConfirm" });
                GameObject child = clone.transform.Find("NameInput").gameObject;
                Templates.RemoveChildren(child, new[] { "Bg", "Title" });
                Templates.AutoUploadTextArea = clone;
            }
        }
    }
}