using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib.BattleDetails
{
    public class TurnObj
    {
        public int Round { get; internal set; }
        public int Turn { get; internal set; }
        public string Id { get; internal set; }
#nullable enable
        public List<CardObj>? Cards { get; internal set; }
        public List<IntentionObj>? Intentions { get; internal set; }
#nullable disable
        public BattleStatusObj Status { get; internal set; }
        public List<StatusEffectObj> StatusEffects { get; internal set; }
    }
}