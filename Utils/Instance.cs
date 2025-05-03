using RunLogger.Utils.RunLogLib;

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
        internal string MiyoiBartenderExhibit;
        internal string YachieOppressionExhibit;
        internal int? DebutUncommonCardsIndex;
        internal bool DebutUncommonCardsChosen;
    }
}