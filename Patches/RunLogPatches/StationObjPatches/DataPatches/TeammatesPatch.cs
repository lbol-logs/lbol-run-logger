using HarmonyLib;
using LBoL.EntityLib.EnemyUnits.Opponent;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.BattleDetails;
using System;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class TeammatesPatch
    {
        [HarmonyPatch(typeof(Cirno), nameof(Cirno.SetNextBuff)), HarmonyPrefix]
        private static void AddTeammates(Cirno __instance)
        {
            Cirno cirno = __instance;
            Type buff = cirno.NextBuff;
            if (buff == null) return;

            string id = buff.Name;
            int level = cirno.NextLevel;
            StatusEffectObj statusEffectObj = new StatusEffectObj()
            {
                Id = id,
                Level = level
            };
            Helpers.AddDataListItem("Teammates", statusEffectObj);
        }
    }
}