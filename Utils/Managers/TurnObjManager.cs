using LBoL.Core.Units;
using RunLogger.Utils.RunLogLib.BattleDetails;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Utils.Managers
{
    internal static class TurnObjManager
    {
        internal static void GetLastTurnObj(out TurnObj turnObj)
        {
            List<TurnObj> details = Controller.CurrentStation.Data["Details"] as List<TurnObj>;
            turnObj = details[^1];
        }

        internal static void AppendTurnObj(int round, int turn, string id, List<CardObj> cards = null)
        {
            TurnObj turnObj = new TurnObj()
            {
                Round = round,
                Turn = turn,
                Id = id,
                Cards = cards
            };
            Helpers.AddDataListItem("Details", turnObj);
        }

        internal static void UpdateTurnObj(Unit unit)
        {
            TurnObjManager.GetLastTurnObj(out TurnObj turnObj);
            turnObj.Status = TurnObjManager.GetStatus(unit);
            turnObj.StatusEffects = TurnObjManager.GetStatusEffects(unit);
        }

        private static BattleStatusObj GetStatus(Unit unit)
        {
            int hp = unit.Hp;
            int block = unit.Block;
            int barrier = unit.Shield;
            BattleStatusObj status = new BattleStatusObj()
            {
                Hp = hp,
                Block = block,
                Barrier = barrier
            };
            if (unit is PlayerUnit playerUnit) status.Power = playerUnit.Power;
            return status;
        }

        private static List<StatusEffectObj> GetStatusEffects(Unit unit)
        {
            List<StatusEffectObj> statusEffects = unit.StatusEffects.Select(se =>
            {
                StatusEffectObj statusEffect = new StatusEffectObj() { Id = se.Id };
                if (se.HasLevel) statusEffect.Level = se.Level;
                if (se.HasDuration) statusEffect.Duration = se.Duration;
                if (se.HasCount) statusEffect.Count = se.Count;
                int? limit = se.Limit;
                if (limit != null && limit != 0) statusEffect.Limit = se.Limit;
                return statusEffect;
            }).ToList();
            return statusEffects;
        }
    }
}