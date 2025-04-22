using LBoL.Base.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RunLogger.Utils
{
    public static class FileManager
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
        private static string GetFilePath(string filename, string subDir)
        {
            string dir = GetDirectory(subDir);
            string path = Path.Combine(dir, filename);
            return path;
        }

        public static string ReadFile(string filename, string subDir)
        {
            string path = FileManager.GetFilePath(filename, subDir);
            if (!File.Exists(path)) return null;
            string text = File.ReadAllText(path);
            return text;
        }

        public static void WriteFile(string filename, string text, string subDir)
        {
            string path = FileManager.GetFilePath(filename, subDir);
            File.WriteAllText(path, text);
        }
    }
}