using HarmonyLib;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils.GameObjects;
using TMPro;
using UnityEngine;

namespace RunLogger.Patches.Panels
{
    [HarmonyPatch]
    internal static class ProfilePanelPatch
    {
        [HarmonyPatch(typeof(ProfilePanel), nameof(ProfilePanel.Awake)), HarmonyPostfix]
        private static void CreateComponents(ProfilePanel __instance)
        {
            if (Templates.AutoUploadEdit != null && Templates.AutoUploadTextArea != null && Templates.AutoUploadEdit != null) return;
            BepinexPlugin.log.LogDebug("ProfilePanel Awaked");
            ProfilePanel panel = __instance;
            if (Templates.AutoUploadEdit == null)
            {
                GameObject clone = Templates.Create(panel, "Profiles/Layout/ProfileWidget0/Content/EditButton");
                clone.name = Templates.Names.Edit;
                Templates.AutoUploadEdit = clone;
            }
            if (Templates.AutoUploadTextArea == null)
            {
                GameObject clone = Templates.Create(panel, "NameInput");
                clone.name = Templates.Names.TextArea;
                Templates.RemoveChildren(clone, new[] { "Bg", "Title" });
                clone.SetActive(false);
                Templates.AutoUploadTextArea = clone;
            }
            if (Templates.AutoUploadUpload == null)
            {
                GameObject clone = Templates.Create(panel, "NameInput/Confirm");
                clone.name = Templates.Names.Upload;
                Templates.ChangeText(clone, "Layout/Text (TMP)", "Upload");
                Templates.AutoUploadUpload = clone;
            }
        }
    }
}