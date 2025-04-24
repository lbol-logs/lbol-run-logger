using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib.BattleDetails
{
    internal class TurnObj
    {
        internal int Round { get; set; }
        internal int Turn { get; set; }
        internal string Id { get; set; }
#nullable enable
        internal List<CardObj>? Cards { get; set; }
        internal List<IntentionObj>? Intentions { get; set; }
#nullable disable
        internal BattleStatusObj Status { get; set; }
        internal List<StatusEffectObj> StatusEffects { get; set; }
    }
}