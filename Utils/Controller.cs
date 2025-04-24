using RunLogger.Utils.RunLogLib;

namespace RunLogger.Utils
{
    internal static class Controller
    {
        internal static Instance Instance { get; private set; }

        internal static void CreateInstance(RunLog runLog)
        {
            Controller.Instance = new Instance(runLog);
        }

        internal static void DestroyInstance()
        {
            Controller.Instance = null;
        }
    }
}