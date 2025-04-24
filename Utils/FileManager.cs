using LBoL.Base.Extensions;
using System.Collections.Generic;
using System.IO;

namespace RunLogger.Utils
{
    internal static class FileManager
    {
        private static string GetDirectory(string subDir)
        {
            List<string> dirs = new List<string>() { Configs.RunLoggerDirName };
            if (!subDir.IsNullOrEmpty()) dirs.Add(Configs.TempDirName);
            string[] paths = dirs.ToArray();
            string path = Path.Combine(paths);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
        internal static string GetFilePath(string filename, string subDir)
        {
            string dir = GetDirectory(subDir);
            string path = Path.Combine(dir, filename);
            return path;
        }

        internal static string ReadFile(string path)
        {
            if (!File.Exists(path)) return null;
            string text = File.ReadAllText(path);
            return text;
        }

        internal static void WriteFile(string path, string text)
        {
            File.WriteAllText(path, text);
        }
    }
}