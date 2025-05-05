using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using LBoL.Core.StatusEffects;
using LBoL.Core.Intentions;
using RunLogger.Legacy.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.BattleDetailsPatches
{
    [HarmonyPatch(typeof(Seija))]
    public static class _SeijaPatch
    {
        //[HarmonyPatch(typeof(BattleAction))]
        //public static class BattleActionPatch
        //{
        //    public static bool isPlayerTrunStarted;

        //    [HarmonyPatch]
        //    class CreateEventPhasePatch
        //    {
        //        static MethodBase TargetMethod()
        //        {
        //            return AccessTools.Method(typeof(BattleAction), nameof(BattleAction.CreateEventPhase)).MakeGenericMethod(typeof(UnitEventArgs));
        //        }
        //        static void Prefix(BattleAction __instance, string name, UnitEventArgs args)
        //        {
        //            BattleController battleController = __instance.Battle;
        //            string enemyGroupid = battleController.EnemyGroup.Id;
        //            if (!RunDataController.enemiesShowDetails.Contains(enemyGroupid)) return;

        //            if (name == "TurnStarted")
        //            {
        //                if (__instance is StartPlayerTurnAction)
        //                {
        //                    isPlayerTrunStarted = true;
        //                }
        //            }
        //            else if (name == "TurnEnded")
        //            {
        //                if (__instance is EndPlayerTurnAction)
        //                {
        //                    PlayerUnit player = args.Unit as PlayerUnit;
        //                    AddDetails(player);
        //                }
        //                else if (__instance is EndEnemyTurnAction)
        //                {
        //                    EnemyUnit enemy = args.Unit as EnemyUnit;
        //                    string id = enemy.Id;
        //                    if (!RunDataController.enemiesShowDetails.Contains(id)) return;

        //                    int round = battleController.RoundCounter;
        //                    int turn = enemy.TurnCounter;

        //                    TurnObj details = new TurnObj()
        //                    {
        //                        Round = round,
        //                        Turn = turn,
        //                        Id = id
        //                    };
        //                    RunDataController.AddDataItem("Details", details);
        //                    AddDetails(enemy);
        //                }
        //            }
        //        }
        //    }

        //    [HarmonyPatch(nameof(BattleAction.CreatePhase), new Type[] { typeof(string), typeof(Action), typeof(bool) }), HarmonyPrefix]
        //    static void CreatePhasePatch(BattleAction __instance, string name)
        //    {
        //        if (!isPlayerTrunStarted) return;
        //        if (name != "InTurn") return;
        //        BattleController battleController = __instance.Battle;
        //        int round = battleController.RoundCounter;
        //        int turn = battleController.Player.TurnCounter;
        //        IReadOnlyList<Card> hands = battleController.HandZone;
        //        List<CardObj> cards = RunDataController.GetCards(hands);

        //        TurnObj details = new TurnObj()
        //        {
        //            Round = round,
        //            Turn = turn,
        //            Id = "Player",
        //            Cards = cards
        //        };
        //        RunDataController.AddDataItem("Details", details);

        //        isPlayerTrunStarted = false;
        //    }
        //}
    }
}
