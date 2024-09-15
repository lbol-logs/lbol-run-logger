using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.StatusEffects.Enemy.SeijaItems;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using RunLogger.Debug;

namespace RunLogger.Patches
{
    [HarmonyPatch(typeof(Seija))]
    public static class SeijaPatch
    {
        private static bool isBattleStart;

        [HarmonyPatch(nameof(Seija.OnEnterBattle)), HarmonyPostfix]
        static void OnEnterBattlePatch(Seija __instance)
        {
            int round = 0;
            int turn = 0;
            string id = __instance.Id;

            TurnObj details = new TurnObj()
            {
                Round = round,
                Turn = turn,
                Id = id
            };
            RunDataController.AddDataItem("Details", details);
            isBattleStart = true;
        }

        [HarmonyPatch(nameof(Seija.GetTurnMoves)), HarmonyPostfix]
        static void GetTurnMovesPatch(Seija __instance)
        {
            if (!isBattleStart) return;
            int hp = __instance.Hp;
            GetDetails(out TurnObj details);
            BattleStatusObj Status = new BattleStatusObj()
            {
                Hp = hp,
                Block = 0,
                Barrier = 0
            };
            details.Status = Status;
            isBattleStart = false;
        }

        [HarmonyPatch(nameof(Seija.RandomBuff)), HarmonyPostfix]
        static void RandomBuffPatch(BattleAction __result, Seija __instance)
        {
            ApplyStatusEffectAction applyStatusEffectAction = __result as ApplyStatusEffectAction;
            StatusEffectApplyEventArgs args = applyStatusEffectAction.Args;
            string se = args.Effect.Id;
            //AddDetails("Se", se);
        }

        public static void GetDetails(out TurnObj details)
        {
            List<TurnObj> Details = RunDataController.CurrentStation.Data["Details"] as List<TurnObj>;
            details = Details[^1];
        }
    }

    [HarmonyPatch(typeof(DragonBallSe))]
    class DragonBallSePatch
    {
        [HarmonyPatch(nameof(DragonBallSe.OnAdded)), HarmonyPostfix]
        static void OnAddedPatch(DragonBallSe __instance)
        {
            //SeijaPatch.AddDetails("Se", __instance.Id);
        }
    }

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
    //            Debugger.Write(name);
    //            if (name == "TurnStarted")
    //            {
    //                if (__instance is StartPlayerTurnAction startPlayerTurnAction)
    //                {
    //                    BattleController battleController = __instance.Battle;
    //                    string id = battleController.EnemyGroup.Id;
    //                    if (!RunDataController.enemiesShowDetails.Contains(id)) return;

    //                    isPlayerTrunStarted = true;
    //                }
    //                else if (__instance is StartEnemyTurnAction startEnemyTurnAction)
    //                {
    //                    EnemyUnit enemy = args.Unit as EnemyUnit;
    //                    string id = enemy.Id;
    //                    if (!RunDataController.enemiesShowDetails.Contains(id)) return;
    //                    BattleController battleController = __instance.Battle;
    //                    int round = battleController.RoundCounter;
    //                    int turn = enemy.TurnCounter;
    //                    int hp = enemy.Hp;

    //                    Dictionary<string, object> details = new Dictionary<string, object>()
    //                    {
    //                        { "Round", round },
    //                        { "Turn", turn },
    //                        { "Id", id },
    //                        { "Hp", hp }
    //                    };
    //                    RunDataController.AddDataItem("Details", details);
    //                }
    //            }
    //            else if (name == "TurnEnded")
    //            {
    //                if (__instance is EndPlayerTurnAction endPlayerTurnAction)
    //                {
    //                    PlayerUnit player = args.Unit as PlayerUnit;
    //                    string id = "Player";
    //                    int hp = player.Hp;

    //                details = new Dictionary<string, object>()
    //                    {

    //                        { "Id", id },
    //                        { "Hp", hp }
    //                    };


    //                details.Add("Cards", cards);
    //                RunDataController.AddDataItem("Details", details);

    //                details = null;
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

    //        details = new Dictionary<string, object>()
    //        {
    //            { "Round", round },
    //            { "Turn", turn },
    //            { "Cards", cards}
    //        };

    //        isPlayerTrunStarted = false;
    //    }
    //}
}
