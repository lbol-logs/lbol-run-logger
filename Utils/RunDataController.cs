﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Stations;
using LBoL.Core.Adventures;
using LBoL.Core.Units;
using LBoL.Core.Dialogs;
using RunLogger.Patches;

namespace RunLogger.Utils
{
    public static class RunDataController
    {
        private const string _dir = "runLogger";
        private static readonly string _path = Path.Join(_dir, "temp.txt");
        private static bool _initialized;

        public static RunData RunData;

        public static List<CardObj> Cards;
        public static List<string> Exhibits;

        public static string[] enemiesShowDetails = new [] { "Seija" };

        public static StationObj CurrentStation
        {
            get {
                if (RunData == null) return null;
                StationObj station = RunData.Stations.LastOrDefault();
                return station;
            }
        }

        public static int CurrentStationIndex
        {
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

        public static void AddListItem2Obj<T>(string key, T listItem)
        {
            key += "s";
            if (!RunDataController.CurrentStation.Rewards.ContainsKey(key)) RunDataController.CurrentStation.Rewards[key] = new List<T>();
            (RunDataController.CurrentStation.Rewards[key] as List<T>).Add(listItem);
        }

        public static void AddCardChange(IEnumerable<Card> cards, ChangeType Type)
        {
            foreach (Card card in cards)
            {
                CardChange Card = new CardChange
                {
                    Id = card.Id,
                    IsUpgraded = card.IsUpgraded,
                    UpgradeCounter = card.UpgradeCounter,
                    Type = Type.ToString(),
                    Station = CurrentStationIndex
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

        public static CardWithPrice GetCardWithPrice(Card card, int? price = null)
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

        public static string GetEnemyGroupId(Station station)
        {
            EnemyGroup enemyGroup;
            if (station is BattleStation battleStation)
            {
                enemyGroup = battleStation.EnemyGroup;
                return enemyGroup.Id;
            }
            else
            {
                return null;
            }
        }

        public static string GetAdventureId(Station station)
        {
            Adventure adventure;
            if (station is AdventureStation adventureStation)
            {
                adventure = adventureStation.Adventure;
                return adventure.Id;
            }
            else if (station is EntryStation entryStation)
            {
                adventure = entryStation.DebutAdventure;
                if (adventure == null) return null;
                return adventure.Id;
            }
            else if (station is TradeStation tradeStation)
            {
                adventure = tradeStation.Adventure;
                return adventure.Id;
            }
            else
            {
                return null;
            }
        }

        public static List<T> GetList<T>(DialogStorage storage, IEnumerable<int> keys, string prefix, string suffix = "")
        {
            List<T> ids = new List<T>();
            foreach (int key in keys)
            {
                string _key = $"{prefix}{key}{suffix}";
                storage.TryGetValue(_key, out object id);
                ids.Add((T)id);
            }
            return ids;
        }

        public static void Reset()
        {
            InteractionViewerPatch.Listener = null;
            StationPatch.RewardListener = null;
            StationPatch.AddRewardsPatch.isAfterAddRewards = false;
            GameRunControllerPatch.isAfterBossReward = false;
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

        public static bool Restore()
        {
            _CheckDirectory();
            if (!File.Exists(_path))
            {
                _initialized = false;
                return false;
            }
            using (FileStream fileStream = File.Open(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(fileStream))
                {
                    string jsonString = streamReader.ReadToEnd();
                    _Decode(jsonString);
                }
            }
            return _initialized;
        }

        public static void Copy(string name)
        {
            if (!_initialized) return;
            Save(false);
            File.Copy(_path, Path.Join(_dir, $"{name}.json"));
            _initialized = false;
        }

        private static void _Write(string line)
        {
            _CheckDirectory();
            if (!_initialized) return;
            using (FileStream fileStream = File.Open(_path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.WriteLine(line);
                }
            }
        }

        private static void _CheckDirectory()
        {
            if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);
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
