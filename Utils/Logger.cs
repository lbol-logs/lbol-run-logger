using Newtonsoft.Json;
using RunLogger.Utils.Enums;
using RunLogger.Utils.RunLogLib;
using System;
using System.IO;

namespace RunLogger.Utils
{
    internal static class Logger
    {
        internal static string Encode(RunLog runLog, bool addIndent)
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
            catch(Exception e)
            {
                BepinexPlugin.log.LogDebug(e);
                Logger.DeleteTemp();
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
            string name = Helpers.CurrentSaveIndex;
            string subDir = Configs.TempDirName;
            string path = FileManager.GetFilePath(name, FilenameExtension.Temp, subDir);
            return path;
        }

        private static string GetLogPath(string name)
        {
            bool saveTogether = BepinexPlugin.saveTogether.Value;
            string subDir = saveTogether ? null : Helpers.CurrentSaveIndex;
            string path = FileManager.GetFilePath(name, FilenameExtension.Log, subDir);
            BepinexPlugin.log.LogDebug($"Log saved: {path}");
            return path;
        }

        internal static void SaveTemp(TempSaveTiming tempSaveTiming)
        {
            string path = GetTempPath();
            Write(path, true);
            BepinexPlugin.log.LogDebug($"Temp save timing: {tempSaveTiming}");
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

        internal static void SaveLog(string name)
        {
            string path = GetLogPath(name);
            Write(path, false);
            Controller.Instance.Path = path;
        }

        internal static void DeleteLog(string path)
        {
            File.Delete(path);
        }
    }
}