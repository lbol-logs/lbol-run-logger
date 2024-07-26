using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;

namespace RunLogger.Utils
{
    public static class RunDataController
    {
        private const string _dir = "runLogger";
        private static readonly string _path = $"{_dir}/temp.json";
        private static bool _initialized;
        private static FileStream _fileStream;
        private static StreamWriter _streamWriter;
        public static RunData RunData;

        static public Station CurrentStation
        {
            get {
                Station station = RunData.Stations.Last();
                return station;
            }
        }

        static public bool ShowRandom
        {
            get
            {
                bool ShowRandomResult = RunData.Info.ShowRandomResult;
                return ShowRandomResult;
            }
        }

        public static void Create()
        {
            _Initialize();
        }

        public static void Save()
        {
            Debugger.Write("trysave");
            if (!_initialized) return;
            string jsonString = _Encode();
            Debugger.Write(jsonString);
            _Write(jsonString);
            Debugger.Write("save done");
        }

        public static void Restore()
        {
            Debugger.Write("tryload");
            if (!File.Exists(_path))
            {
                Create();
                return;
            }
            _Initialize();
            using (StreamReader streamReader = new StreamReader(_fileStream))
            {
                string jsonString = streamReader.ReadToEnd();
                Debugger.Write("loaded: " + jsonString);
                _Decode(jsonString);
            }
            Debugger.Write("load done");
        }

        private static void _Initialize()
        {
            FileStream fileStream = File.Open(_path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            _fileStream = fileStream;
            _streamWriter = streamWriter;
            RunData = new RunData();
            _initialized = true;
        }

        private static void _Write(string line)
        {
            _streamWriter.WriteLine(line);
            _streamWriter.Flush();
            _fileStream.Position = 0;
        }

        private static string _Encode()
        {
            string jsonString = JsonConvert.SerializeObject(RunData, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return jsonString;
        }

        private static void _Decode(string jsonString)
        {
            try
            {
                if (!String.IsNullOrEmpty(jsonString)) RunData = JsonConvert.DeserializeObject<RunData>(jsonString);
            }
            catch (Exception e)
            {
                Debugger.Write(jsonString);
                Debugger.Write(e.ToString());
                Debugger.Write("decode failed");
            }
        }
    }
}
