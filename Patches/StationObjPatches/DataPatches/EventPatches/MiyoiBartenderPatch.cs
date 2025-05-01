using HarmonyLib;
using LBoL.Core;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class MiyoiBartenderPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddExhibitAfterBattle(GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            string adventureId = Helpers.GetAdventureId(gameRun.CurrentStation);
            if (adventureId != nameof(MiyoiBartender)) return;
            //MiyoiBartenderPatch.HandleBattle();
        }
    }
}