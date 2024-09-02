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
using System.Reflection;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Seija))]
    public static class SeijaPatch
    {
        [HarmonyPatch(nameof(Seija.RandomBuff)), HarmonyPostfix]
        static void RandomBuffPatch(BattleAction __result, Seija __instance)
        {
            ApplyStatusEffectAction applyStatusEffectAction = __result as ApplyStatusEffectAction;
            StatusEffectApplyEventArgs args = applyStatusEffectAction.Args;
            string se = args.Effect.Id;
            if (RunDataController.CurrentStation.Data == null)
            {
                int round = 0;
                string id = __instance.Id;

                Dictionary<string, object> details = new Dictionary<string, object>()
                {
                    { "Round", round },
                    { "Id", id },
                    { "Se", se }
                };
                RunDataController.AddDataItem("Details", details);
            }
            else
            {
                AddSe(se);
            }
        }

        public static void AddSe(string se)
        {
            List<Dictionary<string, object>> Details = RunDataController.CurrentStation.Data["Details"] as List<Dictionary<string, object>>;
            Dictionary<string, object> details = Details[^1];
            details["Se"] = se;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(DragonBallSe))]
    class DragonBallSePatch
    {
        [HarmonyPatch(nameof(DragonBallSe.OnAdded)), HarmonyPostfix]
        static void OnAddedPatch(DragonBallSe __instance)
        {
            SeijaPatch.AddSe(__instance.Id);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(BattleAction))]
    class BattleActionPatch
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
                IReadOnlyList<Card> hands = battleController.HandZone;
                List<CardObj> cards = RunDataController.GetCards(hands);

                Dictionary<string, object> details = new Dictionary<string, object>()
                {
                    { "Round", round },
                    { "Id", id },
                    { "Hp", hp },
                    { "Cards", cards }
                };
                RunDataController.AddDataItem("Details", details);
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
}
