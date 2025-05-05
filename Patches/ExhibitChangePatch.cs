using HarmonyLib;
using LBoL.Core;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;

namespace RunLogger.Patches
{
    [HarmonyPatch]
    internal static class ExhibitChangePatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix, HarmonyPriority(Priority.Normal)]
        private static void Add(Exhibit exhibit)
        {
            EntitiesManager.AddExhibitChange(exhibit, ChangeType.Add);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LoseExhibit)), HarmonyPostfix]
        private static void Remove(Exhibit exhibit)
        {
            EntitiesManager.AddExhibitChange(exhibit, ChangeType.Remove);
        }
    }
}