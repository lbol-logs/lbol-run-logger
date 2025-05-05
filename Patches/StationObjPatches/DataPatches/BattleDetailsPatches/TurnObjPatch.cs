using HarmonyLib;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.BattleDetailsPatches
{
    [HarmonyPatch]
    internal static class TurnObjPatch
    {
        [HarmonyPatch(typeof(Seija), nameof(Seija.OnEnterBattle)), HarmonyPostfix]
        private static void AppendOnEnterBattle(Seija __instance)
        {
            TurnObjManager.AppendTurnObj(0, 0, __instance.Id);
        }
    }
}