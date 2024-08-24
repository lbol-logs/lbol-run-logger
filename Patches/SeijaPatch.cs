using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.EnemyUnits.Character;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Reflection;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Seija))]
    class SeijaPatch
    {
        [HarmonyPatch(nameof(Seija.RandomBuff)), HarmonyPostfix]
        static void RandomBuffPatch(BattleAction __result, Seija __instance)
        {
            ApplyStatusEffectAction applyStatusEffectAction = __result as ApplyStatusEffectAction;
            StatusEffectApplyEventArgs args = applyStatusEffectAction.Args;
            string exhibit = args.Effect.Id;
            if (RunDataController.CurrentStation.Data == null)
            {
                int round = 0;
                string id = __instance.Id;

                Dictionary<string, object> details = new Dictionary<string, object>()
                {
                    { "Round", round },
                    { "Id", id },
                    { "Exhibit", exhibit }
                };
                RunDataController.AddDataItem("Details", details);
            }
            else
            {
                List<Dictionary<string, object>> Details = RunDataController.CurrentStation.Data["Details"] as List<Dictionary<string, object>>;
                Dictionary<string, object> details = Details[Details.Count - 1];
                details["Exhibit"] = exhibit;
            }
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
            if (__instance is StartPlayerTurnAction)
            {
                StartPlayerTurnAction startPlayerTurnAction = __instance as StartPlayerTurnAction;
                bool isExtra = startPlayerTurnAction.IsExtra;
                if (isExtra) return;

                BattleController battleController = __instance.Battle;
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
            else if (__instance is StartEnemyTurnAction)
            {
                StartEnemyTurnAction startEnemyTurnAction = __instance as StartEnemyTurnAction;

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
