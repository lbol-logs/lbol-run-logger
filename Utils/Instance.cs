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
    }
}