using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using LBoL.Core.Cards;
using LBoL.Core;

namespace RunLogger.Utils
{
    public static class RunDataController
    {
        private const string _dir = "runLogger";
        private static readonly string _path = $"{_dir}/temp.json";
        private static bool _initialized;
        public static RunData RunData;

        static public StationObj CurrentStation
        {
            get {
                StationObj station = RunData.Stations.LastOrDefault();
                return station;
            }
        }

        static public bool ShowRandom
        {
            get
            {
                bool ShowRandomResult = RunData.Settings.ShowRandomResult;
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

        static public void AddListItem2Obj<T>(ref Dictionary<string, object> dict, string key, T listItem)
        {
            key += "s";
            if (!dict.ContainsKey(key)) dict[key] = new List<T>();
            (dict[key] as List<T>).Add(listItem);
        }

        static public void AddCardChange(Card[] cards, ChangeType Type)
        {
            foreach (Card card in cards)
            {
                CardChange Card = new CardChange
                {
                    Name = card.Id,
                    Type = Type.ToString(),
                    Position = CurrentStation.Position,
                    IsUpgraded = card.IsUpgraded,
                    UpgradeCounter = card.UpgradeCounter
                };
                RunData.Cards.Add(Card);
            }
        }

        static public void AddExhibitChange(Exhibit exhibit, ChangeType Type)
        {
            ExhibitChange Exhibit = new ExhibitChange
            {
                Name = exhibit.Id,
                Type = Type.ToString(),
                Position = CurrentStation.Position
            };
            RunData.Exhibits.Add(Exhibit);
        }

        public static void Create()
        {
            _Write("{}");
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
                _initialized = false;
                return;
            }
            using (FileStream fileStream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string jsonString = streamReader.ReadToEnd();
                    _Decode(jsonString);
                    _initialized = true;
                }
            }
        }

        public static void Copy(string name)
        {
            _initialized = false;
            File.Copy(_path, $"{_dir}/{name}.json");
            _Write("{}");
        }

        private static void _Write(string line)
        {
            if (!_initialized)
            {
                RunData = new RunData();
                _initialized = true;
            }
            using (FileStream fileStream = File.Open(_path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine(line);
                }
            }
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
            catch (Exception)
            {
                _initialized = false;
            }
        }
    }
}
