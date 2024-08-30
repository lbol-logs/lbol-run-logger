using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.JadeBoxes;

namespace RunLogger.Utils
{
    public static class RunDataController
    {
        private const string _dir = "runLogger";
        private static readonly string _path = $"{_dir}/temp.json";
        private static bool _initialized;

        public static RunData RunData;
        public static bool isInitialize;

        public static string Listener;
        public static List<CardObj> Cards;
        public static List<string> Exhibits;

        public static List<string> enemiesShowDetails = new List<string>() { "Seija" };

        public static StationObj CurrentStation
        {
            get {
                if (RunData == null) return null;
                StationObj station = RunData.Stations.LastOrDefault();
                return station;
            }
        }

        public static int CurrentStationIndex {
            get {
                return RunData.Stations.Count - 1;
            }
        }

        public static bool ShowRandom
        {
            get
            {
                bool ShowRandomResult = RunData.Settings.ShowRandomResult;
                return ShowRandomResult;
            }
        }
        public static void GetData(out Dictionary<string, object> Data)
        {
            Data = CurrentStation.Data;
            if (Data == null) Data = (CurrentStation.Data = new Dictionary<string, object>());
        }

        public static void AddData<T>(string key, T value)
        {
            GetData(out Dictionary<string, object> Data);
            Data.Add(key, value);
        }

        public static void AddDataItem<T>(string key, T item)
        {
            GetData(out Dictionary<string, object> Data);
            List<T> list = Data.TryGetValue(key, out object value) ? (List<T>)value : new List<T>();
            list.Add(item);
            Data[key] = list;
        }

        public static void AddPrice(string key, int price)
        {
            GetData(out Dictionary<string, object> Data);
            Data.TryGetValue("Prices", out object value);
            Dictionary<string, int> prices = value as Dictionary<string, int>;
            prices.Add(key, price);
        }

        public static void AddListItem2Obj<T>(ref Dictionary<string, object> dict, string key, T listItem)
        {
            key += "s";
            if (!dict.ContainsKey(key)) dict[key] = new List<T>();
            (dict[key] as List<T>).Add(listItem);
        }

        public static void AddCardChange(Card[] cards, ChangeType Type)
        {
            foreach (Card card in cards)
            {
                CardChange Card = new CardChange
                {
                    Id = card.Id,
                    Type = Type.ToString(),
                    Station = CurrentStationIndex,
                    IsUpgraded = card.IsUpgraded,
                    UpgradeCounter = card.UpgradeCounter
                };
                RunData.Cards.Add(Card);
            }
        }

        public static void AddExhibitChange(Exhibit exhibit, ChangeType Type, int? Counter = null)
        {
            if (RunData == null) return;
            ExhibitChange Exhibit = new ExhibitChange
            {
                Id = exhibit.Id,
                Type = Type.ToString(),
                Station = CurrentStationIndex
            };
            if (Counter != null) Exhibit.Counter = Counter;
            RunData.Exhibits.Add(Exhibit);
        }

        public static CardObj GetCard(Card card)
        {
            CardObj Card = new CardObj
            {
                Id = card.Id,
                IsUpgraded = card.IsUpgraded,
                UpgradeCounter = card.UpgradeCounter
            };
            return Card;
        }

        public static List<CardObj> GetCards(IEnumerable<Card> cards)
        {
            return cards.Select(card => GetCard(card)).ToList();
        }

        public static CardWithPrice GetCardWithPrice(Card card, int price)
        {
            CardWithPrice Card = new CardWithPrice
            {
                Id = card.Id,
                IsUpgraded = card.IsUpgraded,
                UpgradeCounter = card.UpgradeCounter,
                Price = price
            };
            return Card;
        }

        public static string GetBaseMana(string baseMana, IEnumerable<string> exhibits)
        {
            foreach (string exhibit in exhibits)
            {
                ExhibitConfig config = ExhibitConfig.FromId(exhibit);
                Rarity rarity = config.Rarity;
                if (rarity != Rarity.Shining) continue;
                ManaColor? manaColor = config.BaseManaColor;
                if (manaColor == null) baseMana += "A";
            }
            return baseMana;
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
