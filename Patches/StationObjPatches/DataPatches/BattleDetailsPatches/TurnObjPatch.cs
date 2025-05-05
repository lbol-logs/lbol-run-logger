using HarmonyLib;
using LBoL.Core.Battle;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;
using System;
using System.Collections.Generic;

namespace RunLogger.Patches.StationObjPatches.DataPatches.BattleDetailsPatches
{
    [HarmonyPatch]
    internal static class TurnObjPatch
    {
        [HarmonyPatch(typeof(Seija), nameof(Seija.OnEnterBattle)), HarmonyPostfix]
        private static void AppendEnemyOnEnterBattle(Seija __instance)
        {
            TurnObjManager.AppendTurnObj(0, 0, __instance.Id);
        }

        [HarmonyPatch(typeof(BattleAction), nameof(BattleAction.CreatePhase), new Type[] { typeof(string), typeof(Action), typeof(bool) }), HarmonyPrefix]
        private static void AppendPlayer(string name, BattleAction __instance)
        {
            if (name != "InTurn") return;
            BattleController battle = __instance.Battle;
            if (battle.EnemyGroup.Id != nameof(Seija)) return;
            if (battle.PlayerTurnShouldEnd) return;

            int round = battle.RoundCounter;
            int turn = battle.Player.TurnCounter;
            List<CardObj> cards = Helpers.ParseCards(battle.HandZone);
            TurnObjManager.AppendTurnObj(round, turn, "Player", cards);
        }
    }
}