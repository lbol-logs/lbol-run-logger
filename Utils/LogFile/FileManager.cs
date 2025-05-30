﻿using LBoL.Base.Extensions;
using System.Collections.Generic;
using System.IO;

namespace RunLogger.Utils.LogFile
{
    internal static class FileManager
    {
        private static string GetDirectory(string subDir)
        {
            List<string> dirs = new List<string>() { Configs.RunLoggerDirName };
            if (!subDir.IsNullOrEmpty()) dirs.Add(subDir);
            string[] paths = dirs.ToArray();
            string path = Path.Combine(paths);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
        internal static string GetFilePath(string name, string extension, string subDir)
        {
            string dir = GetDirectory(subDir);
            string filename = $"{name}.{extension}";
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