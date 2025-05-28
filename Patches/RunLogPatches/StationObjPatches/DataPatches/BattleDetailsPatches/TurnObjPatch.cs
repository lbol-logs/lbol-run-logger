using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Utils;
using RunLogger.Utils.Managers;
using RunLogger.Utils.RunLogLib.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.BattleDetailsPatches
{
    [HarmonyPatch]
    internal static class TurnObjPatch
    {
        [HarmonyPatch(typeof(Seija), nameof(Seija.OnEnterBattle)), HarmonyPostfix]
        private static void AppendOnEnemyEnterBattle(Seija __instance)
        {
            if (!Instance.IsInitialized) return;
            TurnObjManager.AppendTurnObj(0, 0, __instance.Id);
        }

        [HarmonyPatch(typeof(BattleAction), nameof(BattleAction.CreatePhase), new Type[] { typeof(string), typeof(Action), typeof(bool) }), HarmonyPrefix]
        private static void AppendOnPlayerStartTurn(string name, BattleAction __instance)
        {
            if (!Instance.IsInitialized) return;

            if (name != "InTurn") return;
            BattleController battle = __instance.Battle;
            if (battle.EnemyGroup.Id != nameof(Seija)) return;
            if (battle.PlayerTurnShouldEnd) return;

            int round = battle.RoundCounter;
            int turn = battle.Player.TurnCounter;
            List<CardObj> cards = Helpers.ParseCards(battle.HandZone);
            TurnObjManager.AppendTurnObj(round, turn, "Player", cards);
        }

        [HarmonyPatch]
        private static class UpsertOnPlayerAndEnemyEndTurn
        {
            private static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(BattleAction), nameof(BattleAction.CreateEventPhase)).MakeGenericMethod(typeof(UnitEventArgs));
            }
            private static void Prefix(string name, UnitEventArgs args, BattleAction __instance)
            {
                if (!Instance.IsInitialized) return;

                if (name != "TurnEnded") return;
                BattleAction battleAction = __instance;
                BattleController battle = battleAction.Battle;
                if (battle.EnemyGroup.Id != nameof(Seija)) return;

                if (battleAction is EndPlayerTurnAction)
                {
                    PlayerUnit player = args.Unit as PlayerUnit;
                    TurnObjManager.UpdateTurnObj(player);
                }
                else if (battleAction is EndEnemyTurnAction)
                {
                    EnemyUnit enemy = args.Unit as EnemyUnit;
                    string id = enemy.Id;
                    if (id != nameof(Seija)) return;

                    int round = battle.RoundCounter;
                    int turn = enemy.TurnCounter;

                    TurnObjManager.AppendTurnObj(round, turn, id);
                    TurnObjManager.UpdateTurnObj(enemy);
                }
            }
        }
    }
}