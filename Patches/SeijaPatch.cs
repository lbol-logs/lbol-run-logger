﻿using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using LBoL.Core.StatusEffects;
using LBoL.Core.Intentions;

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
        static void GetTurnMovesPatch(Seija __instance, IEnumerable<IEnemyMove> __result)
        {
            if (isBattleStart)
            {
                AddDetails(__instance);
                isBattleStart = false;
            }

            AddIntentions(__result, __instance);
        }

        private static void AddDetails(Unit unit)
        {
            GetDetails(out TurnObj details);
            BattleStatusObj Status = GetStatus(unit);
            details.Status = Status;
            List<StatusEffectObj> StatusEffects = GetStatusEffects(unit);
            details.StatusEffects = StatusEffects;
        }

        private static void GetDetails(out TurnObj details)
        {
            List<TurnObj> Details = RunDataController.CurrentStation.Data["Details"] as List<TurnObj>;
            details = Details[^1];
        }

        private static BattleStatusObj GetStatus(Unit unit)
        {
            int hp = unit.Hp;
            int block = unit.Block;
            int barrier = unit.Shield;
            BattleStatusObj Status = new BattleStatusObj()
            {
                Hp = hp,
                Block = block,
                Barrier = barrier
            };
            if (unit is PlayerUnit playerUnit) Status.Power = playerUnit.Power;
            return Status;
        }

        private static List<StatusEffectObj> GetStatusEffects(Unit unit)
        {
            List<StatusEffectObj> StatusEffects = unit.StatusEffects.Select((StatusEffect se) =>
            {
                StatusEffectObj StatusEffect = new StatusEffectObj() { Id = se.Id };
                if (se.HasLevel) StatusEffect.Level = se.Level;
                if (se.HasDuration) StatusEffect.Duration = se.Duration;
                if (se.HasCount) StatusEffect.Count = se.Count;
                int? limit = se.Limit;
                if (limit != null && limit != 0) StatusEffect.Limit = se.Limit;
                return StatusEffect;
            }).ToList();
            return StatusEffects;
        }

        private static void AddIntentions(IEnumerable<IEnemyMove> moves, Seija enemy)
        {
            GetDetails(out TurnObj details);
            List<IntentionObj> Intentions = moves.Select((IEnemyMove m) =>
            {
                Intention i = m.Intention;
                IntentionType Type = i.Type;
                string type = Type.ToString();
                IntentionObj Intention;

                switch (Type)
                {
                    case IntentionType.Attack:
                        {
                            AttackIntention _i = i as AttackIntention;
                            DamageInfo damageInfo = _i.Damage;
                            int damage = enemy.Battle.CalculateDamage(enemy, enemy, enemy.Battle.Player, damageInfo);
                            Intention = new IntentionObj()
                            {
                                Type = type,
                                Damage = damage,
                                Times = _i.Times,
                                IsAccurate = _i.IsAccuracy
                            };
                            break;
                        }
                    case IntentionType.SpellCard:
                        {
                            SpellCardIntention _i = i as SpellCardIntention;

                            Intention = new IntentionObj()
                            {
                                Type = type
                            };
                            break;
                        }
                    case IntentionType.Clear:
                        {
                            ClearIntention _i = i as ClearIntention;

                            Intention = new IntentionObj()
                            {
                                Type = type
                            };
                            break;
                        }
                    default:
                        {
                            Intention = new IntentionObj();
                            break;
                        }
                }

                return Intention;
            }).ToList();
            details.Intentions = Intentions;
        }

        [HarmonyPatch(typeof(BattleAction))]
        public static class BattleActionPatch
        {
            public static bool isPlayerTrunStarted;

            [HarmonyPatch]
            class CreateEventPhasePatch
            {
                static MethodBase TargetMethod()
                {
                    return AccessTools.Method(typeof(BattleAction), nameof(BattleAction.CreateEventPhase)).MakeGenericMethod(typeof(UnitEventArgs));
                }
                static void Prefix(BattleAction __instance, string name, UnitEventArgs args)
                {
                    BattleController battleController = __instance.Battle;
                    string enemyGroupid = battleController.EnemyGroup.Id;
                    if (!RunDataController.enemiesShowDetails.Contains(enemyGroupid)) return;

                    if (name == "TurnStarted")
                    {
                        if (__instance is StartPlayerTurnAction)
                        {
                            isPlayerTrunStarted = true;
                        }
                    }
                    else if (name == "TurnEnded")
                    {
                        if (__instance is EndPlayerTurnAction)
                        {
                            PlayerUnit player = args.Unit as PlayerUnit;
                            AddDetails(player);
                        }
                        else if (__instance is EndEnemyTurnAction)
                        {
                            EnemyUnit enemy = args.Unit as EnemyUnit;
                            string id = enemy.Id;
                            if (!RunDataController.enemiesShowDetails.Contains(id)) return;

                            int round = battleController.RoundCounter;
                            int turn = enemy.TurnCounter;

                            TurnObj details = new TurnObj()
                            {
                                Round = round,
                                Turn = turn,
                                Id = id
                            };
                            RunDataController.AddDataItem("Details", details);
                            AddDetails(enemy);
                        }
                    }
                }
            }

            [HarmonyPatch(nameof(BattleAction.CreatePhase), new Type[] { typeof(string), typeof(Action), typeof(bool) }), HarmonyPrefix]
            static void CreatePhasePatch(BattleAction __instance, string name)
            {
                if (!isPlayerTrunStarted) return;
                if (name != "InTurn") return;
                BattleController battleController = __instance.Battle;
                int round = battleController.RoundCounter;
                int turn = battleController.Player.TurnCounter;
                IReadOnlyList<Card> hands = battleController.HandZone;
                List<CardObj> cards = RunDataController.GetCards(hands);

                TurnObj details = new TurnObj()
                {
                    Round = round,
                    Turn = turn,
                    Id = "Player",
                    Cards = cards
                };
                RunDataController.AddDataItem("Details", details);

                isPlayerTrunStarted = false;
            }
        }
    }
}
