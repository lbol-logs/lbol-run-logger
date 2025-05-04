using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class YachieOppressionPatch
    {
        [HarmonyPatch(typeof(YachieOppression), nameof(YachieOppression.InitVariables))]
        private static void AddExhibitBeforeBattle(YachieOppression __instance)
        {
            if (!Controller.ShowRandomResult) return;
            YachieOppressionPatch.AddExhibit(__instance.Storage);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddExhibitAfterBattle()
        {
            if (Controller.ShowRandomResult) return;
            if (!Helpers.IsAdventure<YachieOppression>(out DialogStorage storage)) return;
            YachieOppressionPatch.AddExhibit(storage);
        }

        private static void AddExhibit(DialogStorage storage)
        {
            storage.TryGetValue("$enemyExhibit", out string exhibit);
            Helpers.AddDataValue("Exhibit", exhibit);
        }
    }
}