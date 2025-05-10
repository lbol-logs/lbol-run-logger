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
        internal static bool IsInitialized;

        internal int? PreHealHp;
        internal int? BackgroundDancersIndex;
        internal int? DebutUncommonCardsIndex;
        internal bool DebutUncommonCardsChosen;
        internal List<IEnumerable<StationReward>> RewardsBeforeDebut;
    }
}