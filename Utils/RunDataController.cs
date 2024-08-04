using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
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
        public static bool isInitialize;

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
                    Id = card.Id,
                    Type = Type.ToString(),
                    Node = CurrentStation.Node,
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
                Id = exhibit.Id,
                Type = Type.ToString(),
                Node = CurrentStation.Node
            };
            RunData.Exhibits.Add(Exhibit);
        }

        static public void AddExhibitUse(Exhibit exhibit, int Counter)
        {
            // if (RunData == null) return;
            ChangeType Type = ChangeType.Use;
            // if (!RunData.Exhibits.Any(e => e.Type == Type.ToString() && e.Id == exhibit.Id && e.Counter == Counter))
            // {
                ExhibitChange Exhibit = new ExhibitChange
                {
                    Id = exhibit.Id,
                    Type = Type.ToString(),
                    Node = CurrentStation.Node,
                    Counter = Counter
                };
                RunData.Exhibits.Add(Exhibit);
            //}
        }

        public static void Create()
        {
            RunData = new RunData();
            _initialized = true;
            Save();
        }

        public static void Save(bool indent = true)
        {
            if (!_initialized) return;
            string jsonString = _Encode(indent);
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
                }
            }
        }

        public static void Copy(string name)
        {
            if (!_initialized) return;
            Save(false);
            File.Copy(_path, $"{_dir}/{name}.json");
            _initialized = false;
        }

        private static void _Write(string line)
        {
            if (!_initialized) return;
            using (FileStream fileStream = File.Open(_path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine(line);
                }
            }
        }

        private static string _Encode(bool indent = true)
        {
            string jsonString = JsonConvert.SerializeObject(
                RunData,
                indent ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );
            return jsonString;
        }

        private static void _Decode(string jsonString)
        {
            try
            {
                if (!String.IsNullOrEmpty(jsonString))
                {
                    RunData = JsonConvert.DeserializeObject<RunData>(jsonString);
                    _initialized = true;
                    return;
                }
            }
            finally
            {

            }
            _initialized = false;
        }
    }
}
