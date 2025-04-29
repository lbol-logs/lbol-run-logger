using HarmonyLib;
using LBoL.Core;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;

namespace RunLogger.Patches
{
    [HarmonyPatch]
    public static class ExhibitChangePatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix]
        private static void AddExhibitPatch(Exhibit exhibit)
        {
            EntitiesManager.AddExhibitChange(exhibit, ChangeType.Add);
        }
    }
}