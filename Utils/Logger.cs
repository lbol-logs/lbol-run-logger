using Newtonsoft.Json;
using RunLogger.Utils.RunLogLib;
using System.IO;

namespace RunLogger.Utils
{
    internal static class Logger
    {
        private static string Encode(RunLog runLog, bool addIndent)
        {
            string jsonString = JsonConvert.SerializeObject(
                runLog,
                addIndent ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );
            return jsonString;
        }

        private static RunLog Decode(string jsonString)
        {
            try
            {
                if (!string.IsNullOrEmpty(jsonString))
                {
                    RunLog runLog = JsonConvert.DeserializeObject<RunLog>(jsonString);
                    return runLog;
                }
            }
            finally
            {
            }
            return null;
        }

        private static RunLog Read(string path)
        {
            string jsonString = FileManager.ReadFile(path);
            RunLog runLog = Logger.Decode(jsonString);
            return runLog;
        }

        private static void Write(string path, bool addIndent)
        {
            RunLog runLog = Controller.Instance.RunLog;
            if (runLog == null) return;
            string jsonString = Logger.Encode(runLog, addIndent);
            FileManager.WriteFile(path, jsonString);
        }

        private static string GetTempPath()
        {
            //TODO
            string filename = "temp.txt";
            string subDir = null;
            string path = FileManager.GetFilePath(filename, subDir);
            return path;
        }

        private static string GetLogPath(string filename)
        {
            //TODO
            string subDir = null;
            string path = FileManager.GetFilePath(filename, subDir);
            return path;
        }

        internal static void SaveTemp()
        {
            string path = GetTempPath();
            Write(path, true);
        }

        internal static RunLog LoadTemp()
        {
            string path = GetTempPath();
            RunLog runLog = Logger.Read(path);
            return runLog;
        }

        internal static void DeleteTemp()
        {
            string path = GetTempPath();
            File.Delete(path);
        }

        internal static void SaveLog(string filename)
        {
            string path = GetLogPath(filename + ".json");
            Write(path, false);
        }
    }
}