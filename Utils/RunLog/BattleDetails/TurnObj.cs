using RunLogger.Utils.RunLog.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLog.BattleDetails
{
    public class TurnObj
    {
        public int Round { get; set; }
        public int Turn { get; set; }
        public string Id { get; set; }
#nullable enable
        public List<CardObj>? Cards { get; set; }
        public List<IntentionObj>? Intentions { get; set; }
#nullable disable
        public BattleStatusObj Status { get; set; }
        public List<StatusEffectObj> StatusEffects { get; set; }
    }
}