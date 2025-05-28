using LBoL.Core.Stations;
using RunLogger.Utils.RunLogLib;
using System.Collections.Generic;

namespace RunLogger.Utils
{
    internal class Instance
    {
        internal Instance(RunLog runLog)
        {
            this.RunLog = runLog;
        }

        internal RunLog RunLog { get; private set; }
        internal string Path;
        internal static bool IsInitialized;

        internal int? PreHealHp;
        internal int? BackgroundDancersIndex;
        internal int? DebutUncommonCardsIndex;
        internal bool DebutUncommonCardsChosen;

        internal bool? IsForced;
        internal List<IEnumerable<StationReward>> RewardsBeforeDebut;
        internal bool IsAbandoned;
    }
}