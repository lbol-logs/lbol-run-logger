using RunLogger.Utils.RunLogLib;

namespace RunLogger.Utils
{
    internal static class Controller
    {
        private Controller(RunLog runLog)
        {
            this.RunLog = runLog ?? new RunLog();
        }

        public static Controller Instance { get; private set; }
        public bool IsTempLoaded { get; private set; }
        public RunLog RunLog { get; private set; }

        internal static void CreateInstance(RunLog runLog)
        {
            Controller.Instance = new Controller(runLog);
        }

        internal static void DestroyInstance()
        {
            Controller.Instance = null;
        }
    }
}