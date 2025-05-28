using HarmonyLib;
using LBoL.Core;
using LBoL.EntityLib.Exhibits.Adventure;
using RunLogger.Utils;
using RunLogger.Utils.Managers;
using RunLogger.Utils.RunLogLib.Entities;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class HecatiaTshirtPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
        private static void UpgradeExhibit(Exhibit exhibit)
        {
            if (!Instance.IsInitialized) return;

            if (!(exhibit is IdolTshirt)) return;
            int counter = exhibit.Counter;
            if (counter > exhibit.Config.InitialCounter) EntitiesManager.AddExhibitChange(exhibit, ChangeType.Upgrade, counter);
        }
    }
}