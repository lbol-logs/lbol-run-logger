using HarmonyLib;
using LBoL.Core;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class YachieOppressionPatch
    {
        [HarmonyPatch(typeof(YachieOppression), nameof(YachieOppression.InitVariables))]
        private static void AddExhibit(YachieOppression __instance)
        {
            __instance.Storage.TryGetValue("$enemyExhibit", out string exhibit);
            if (Controller.ShowRandomResult) Helpers.AddDataValue("Exhibit", exhibit);
            else Controller.Instance.YachieOppressionExhibit = exhibit;
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddExhibitAfterBattle(GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            string adventureId = Helpers.GetAdventureId(gameRun.CurrentStation);
            if (adventureId != nameof(YachieOppression)) return;
            if (Controller.ShowRandomResult) return;
            Helpers.AddDataValue("Exhibit", Controller.Instance.YachieOppressionExhibit);
            Controller.Instance.YachieOppressionExhibit = null;
        }
    }
}