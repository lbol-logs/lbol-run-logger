﻿using System.IO;
using System.Text;

namespace RunLogger.Utils
{
    public static class Debugger
    {
        private const string _dir = "runLogger";
        private static bool _initialized;
        private static StreamWriter _streamWriter;

        public static void Initialize()
        {
            if (_initialized)
            {
                return;
            }
            Reload();
            _initialized = true;
        }

        public static void Reload()
        {
            Directory.CreateDirectory(_dir);
            FileStream fileStream = File.Open($"{_dir}/debug.txt", FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            _streamWriter = streamWriter;
        }

        public static void Write(string line)
        {
            Initialize();
            _streamWriter.WriteLine(line);
            _streamWriter.Flush();
        }
    }
}