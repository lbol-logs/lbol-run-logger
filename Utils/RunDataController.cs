using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

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
        static public void GetData(out Dictionary<string, object> Data)
        {
            Data = CurrentStation.Data;
            if (Data == null) Data = (CurrentStation.Data = new Dictionary<string, object>());
        }

        static public void AddData<T>(string key, T value)
        {
            GetData(out Dictionary<string, object> Data);
            Data.Add(key, value);
        }

        static public void AddDataItem<T>(string key, T item)
        {
            GetData(out Dictionary<string, object> Data);
            List<T> list = Data.TryGetValue(key, out object value) ? (List<T>)value : new List<T>();
            list.Add(item);
            Data[key] = list;
        }

        public static void Create()
        {
            _Initialize(true);
        }

        public static void Save()
        {
            if (!_initialized) return;
            string jsonString = _Encode();
            _Write(jsonString);
        }

        public static void Restore()
        {
            if (!File.Exists(_path))
            {
                Create();
                return;
            }
            _Initialize();
            StreamReader streamReader = new StreamReader(_fileStream);
            string jsonString = streamReader.ReadToEnd();
            _fileStream.Position = 0;
            _Decode(jsonString);
        }

        private static void _Initialize(bool create = false)
        {
            FileStream fileStream = File.Open(_path, create ? FileMode.Create : FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
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
                Debugger.Write(e.ToString());
                Debugger.Write("decode failed");
            }
        }
    }
}
