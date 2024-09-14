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
            string id = __instance.Id;

            Dictionary<string, object> details = new Dictionary<string, object>()
            {
                { "Round", round },
                { "Id", id }
            };
            RunDataController.AddDataItem("Details", details);
            isBattleStart = true;
        }

        [HarmonyPatch(nameof(Seija.GetTurnMoves)), HarmonyPostfix]
        static void GetTurnMovesPatch(Seija __instance)
        {
            if (!isBattleStart) return;
            string hp = __instance.Hp.ToString();
            AddDetails("Hp", hp);
            isBattleStart = false;
        }

        [HarmonyPatch(nameof(Seija.RandomBuff)), HarmonyPostfix]
        static void RandomBuffPatch(BattleAction __result, Seija __instance)
        {
            ApplyStatusEffectAction applyStatusEffectAction = __result as ApplyStatusEffectAction;
            StatusEffectApplyEventArgs args = applyStatusEffectAction.Args;
            string se = args.Effect.Id;
            AddDetails("Se", se);
        }

        public static void AddDetails(string key, string value)
        {
            List<Dictionary<string, object>> Details = RunDataController.CurrentStation.Data["Details"] as List<Dictionary<string, object>>;
            Dictionary<string, object> details = Details[^1];
            details[key] = value;
        }
    }

    [HarmonyPatch(typeof(DragonBallSe))]
    class DragonBallSePatch
    {
        [HarmonyPatch(nameof(DragonBallSe.OnAdded)), HarmonyPostfix]
        static void OnAddedPatch(DragonBallSe __instance)
        {
            SeijaPatch.AddDetails("Se", __instance.Id);
        }
    }

    [HarmonyPatch(typeof(BattleAction))]
    class BattleActionPatch
    {
        private static bool isPlayerTrunStarted;
        private static Dictionary<string, object> details;

        [HarmonyPatch]
        class CreateEventPhasePatch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(BattleAction), nameof(BattleAction.CreateEventPhase)).MakeGenericMethod(typeof(UnitEventArgs));
            }
            static void Prefix(BattleAction __instance, string name, UnitEventArgs args)
            {
                if (name != "TurnStarted") return;
                if (__instance is StartPlayerTurnAction startPlayerTurnAction)
                {
                    BattleController battleController = __instance.Battle;
                    if (battleController.EnemyGroup.Id != nameof(Seija)) return;
                    bool isExtra = startPlayerTurnAction.IsExtra;
                    if (isExtra) return;

                    int round = battleController.RoundCounter;
                    PlayerUnit player = args.Unit as PlayerUnit;
                    string id = "Player";
                    int hp = player.Hp;

                    details = new Dictionary<string, object>()
                    {
                        { "Round", round },
                        { "Id", id },
                        { "Hp", hp }
                    };

                    isPlayerTrunStarted = true;
                }
                else if (__instance is StartEnemyTurnAction startEnemyTurnAction)
                {
                    EnemyUnit enemy = args.Unit as EnemyUnit;
                    string id = enemy.Id;
                    if (!RunDataController.enemiesShowDetails.Contains(id)) return;
                    BattleController battleController = __instance.Battle;
                    int round = battleController.RoundCounter;
                    int hp = enemy.Hp;

                    Dictionary<string, object> details = new Dictionary<string, object>()
                    {
                        { "Round", round },
                        { "Id", id },
                        { "Hp", hp }
                    };
                    RunDataController.AddDataItem("Details", details);
                }
            }
        }


        [HarmonyPatch(nameof(BattleAction.CreatePhase), new Type[] { typeof(string), typeof(Action), typeof(bool) }), HarmonyPrefix]
        static void CreatePhasePatch(BattleAction __instance, string name)
        {
            if (!isPlayerTrunStarted) return;
            if (name != "InTurn") return;
            BattleController battleController = __instance.Battle;
            IReadOnlyList<Card> hands = battleController.HandZone;
            List<CardObj> cards = RunDataController.GetCards(hands);

            details.Add("Cards", cards);
            RunDataController.AddDataItem("Details", details);

            isPlayerTrunStarted = false;
            details = null;
        }
    }
}
