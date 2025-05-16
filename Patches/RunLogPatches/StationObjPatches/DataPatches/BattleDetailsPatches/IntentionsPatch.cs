using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Intentions;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.BattleDetails;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.BattleDetailsPatches
{
    [HarmonyPatch]
    internal static class IntentionsPatch
    {
        [HarmonyPatch(typeof(Seija), nameof(Seija.GetTurnMoves)), HarmonyPostfix]
        private static void AddIntentions(Seija __instance, IEnumerable<IEnemyMove> __result)
        {
            Seija seija = __instance;
            if (seija.TurnCounter == 0) TurnObjManager.UpdateTurnObj(seija);
            IntentionsPatch.AddIntentionsInternal(seija, __result);
        }

        private static void AddIntentionsInternal(Seija enemy, IEnumerable<IEnemyMove> moves)
        {
            TurnObjManager.GetLastTurnObj(out TurnObj turnObj);
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
        }
    }
}