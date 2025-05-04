using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Legacy.Utils;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class BattleDetailsPatch
    {
        [HarmonyPatch(typeof(Seija), nameof(Seija.OnEnterBattle)), HarmonyPostfix]
        private static void AppendDetailsOnEnterBattle(Seija __instance)
        {
            BattleDetailsPatch.AppendTurnObj(0, 0, __instance.Id);
            //isBattleStart = true;
        }

        [HarmonyPatch(typeof(Seija), nameof(Seija.GetTurnMoves)), HarmonyPostfix]
        private static void GetTurnMovesPatch(Seija __instance, IEnumerable<IEnemyMove> __result)
        {
            BepinexPlugin.log.LogDebug("GetTurnMoves");
            //if (isBattleStart)
            //{
            //    AddDetails(__instance);
            //    isBattleStart = false;
            //}

            BattleDetailsPatch.AddIntentions(__result, __instance);
        }

        private static void AddIntentions(IEnumerable<IEnemyMove> moves, Seija enemy)
        {
            BattleDetailsPatch.GetLastTurnObj(out TurnObj turnObj);
            List<IntentionObj> intentions = moves.Select(m =>
            {
                Intention intention = m.Intention;
                IntentionType intentionType = intention.Type;
                string type = intentionType.ToString();
                IntentionObj intentionObj;

                switch (intentionType)
                {
                    case IntentionType.Attack:
                        {
                            AttackIntention _i = intention as AttackIntention;
                            DamageInfo damageInfo = _i.Damage;
                            int damage = enemy.Battle.CalculateDamage(enemy, enemy, enemy.Battle.Player, damageInfo);
                            intentionObj = new IntentionObj()
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
                            SpellCardIntention _i = intention as SpellCardIntention;
                            intentionObj = new IntentionObj()
                            {
                                Type = type
                            };
                            break;
                        }
                    case IntentionType.Clear:
                        {
                            ClearIntention _i = intention as ClearIntention;
                            intentionObj = new IntentionObj()
                            {
                                Type = type
                            };
                            break;
                        }
                    default:
                        {
                            intentionObj = new IntentionObj();
                            break;
                        }
                }

                return intentionObj;
            }).ToList();
            turnObj.Intentions = intentions;
            BepinexPlugin.log.LogDebug(Newtonsoft.Json.JsonConvert.SerializeObject(turnObj));
        }

        private static void GetLastTurnObj(out TurnObj turnObj)
        {
            List<TurnObj> details = Controller.CurrentStation.Data["Details"] as List<TurnObj>;
            turnObj = details[^1];
        }

        private static void AppendTurnObj(int round, int turn, string id)
        {
            TurnObj turnObj = new TurnObj()
            {
                Round = round,
                Turn = turn,
                Id = id
            };
            Helpers.AddDataListItem("Details", turnObj);
        }
    }
}