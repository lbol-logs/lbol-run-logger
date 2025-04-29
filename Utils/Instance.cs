using LBoL.Core.Cards;
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
    }
}