using RunLogger.Utils.RunLogLib;
using System.Linq;

namespace RunLogger.Utils
{
    internal static class Controller
    {
        internal static Instance Instance { get; private set; }

        internal static void CreateInstance(RunLog runLog)
        {
            Controller.Instance = new Instance(runLog);
            Instance.IsInitialized = true;
        }

        internal static void DestroyInstance()
        {
            Instance.IsInitialized = false;
            Controller.Instance = null;
        }

        internal static StationObj LastStation
        {
            get
            {
                int lastStationIndex = Controller.CurrentStationIndex - 1;
                if (lastStationIndex == -1) return null;
                StationObj station = Controller.Instance.RunLog.Stations[lastStationIndex];
                return station;
            }
        }

        internal static StationObj CurrentStation
        {
            get
            {
                StationObj station = Controller.Instance.RunLog.Stations.LastOrDefault();
                return station;
            }
        }

        internal static int CurrentStationIndex
        {
            get
            {
                int index = Controller.Instance.RunLog.Stations.Count - 1;
                return index;
            }
        }

        internal static bool ShowRandomResult
        {
            get
            {
                bool showRandomResult = Controller.Instance.RunLog.Settings.ShowRandomResult;
                return showRandomResult;
            }
        }
    }
}