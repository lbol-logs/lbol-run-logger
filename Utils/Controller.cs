using LBoL.Base;
using LBoL.ConfigData;
using RunLogger.Utils.RunLogLib;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Utils
{
    internal static class Controller
    {
        internal static Instance Instance { get; private set; }

        internal static void CreateInstance(RunLog runLog)
        {
            Controller.Instance = new Instance(runLog);
            Instance.IsInitialized = true;
        }

        internal static void DestroyInstance()
        {
            Instance.IsInitialized = false;
            Controller.Instance = null;
        }

        //TODO

        internal static StationObj CurrentStation
        {
            get
            {
                StationObj station = Controller.Instance.RunLog.Stations.LastOrDefault();
                return station;
            }
        }

        internal static int CurrentStationIndex
        {
            get
            {
                int index = Controller.Instance.RunLog.Stations.Count - 1;
                return index;
            }
        }

        //internal static int CurrentStationIndex
        //{
        //    get
        //    {
        //        return Controller.Instance.RunLog.Stations.Count - 1;
        //    }
        //}

        //internal static bool ShowRandom
        //{
        //    get
        //    {
        //        bool ShowRandomResult = Controller.Instance.RunLog.Settings.ShowRandomResult;
        //        return ShowRandomResult;
        //    }
        //}

        //public static void GetData(out Dictionary<string, object> Data)
        //{
        //    Data = CurrentStation.Data;
        //    if (Data == null) Data = CurrentStation.Data = new Dictionary<string, object>();
        //}

        //public static void AddData<T>(string key, T value)
        //{
        //    GetData(out Dictionary<string, object> Data);
        //    Data.Add(key, value);
        //}

        //public static void AddDataItem<T>(string key, T item)
        //{
        //    GetData(out Dictionary<string, object> Data);
        //    List<T> list = Data.TryGetValue(key, out object value) ? (List<T>)value : new List<T>();
        //    list.Add(item);
        //    Data[key] = list;
        //}

        //public static void AddPrice(string key, int price)
        //{
        //    GetData(out Dictionary<string, object> Data);
        //    Data.TryGetValue("Prices", out object value);
        //    Dictionary<string, int> prices = value as Dictionary<string, int>;
        //    prices.Add(key, price);
        //}

        //public static void AddListItem2Obj<T>(string key, T listItem)
        //{
        //    key += "s";
        //    if (!CurrentStation.Rewards.ContainsKey(key)) CurrentStation.Rewards[key] = new List<T>();
        //    (CurrentStation.Rewards[key] as List<T>).Add(listItem);
        //}

        //public static void AddCardChange(IEnumerable<Card> cards, ChangeType Type)
        //{
        //    foreach (Card card in cards)
        //    {
        //        CardChange Card = new CardChange
        //        {
        //            Id = card.Id,
        //            IsUpgraded = card.IsUpgraded,
        //            UpgradeCounter = card.UpgradeCounter,
        //            Type = Type.ToString(),
        //            Station = CurrentStationIndex
        //        };
        //        RunData.Cards.Add(Card);
        //    }
        //}

        //public static void AddExhibitChange(Exhibit exhibit, ChangeType Type, int? Counter = null)
        //{
        //    if (RunData == null) return;
        //    ExhibitChange Exhibit = new ExhibitChange
        //    {
        //        Id = exhibit.Id,
        //        Type = Type.ToString(),
        //        Station = CurrentStationIndex
        //    };
        //    if (Counter != null) Exhibit.Counter = Counter;
        //    RunData.Exhibits.Add(Exhibit);
        //}

        //public static CardObj GetCard(Card card)
        //{
        //    CardObj Card = new CardObj
        //    {
        //        Id = card.Id,
        //        IsUpgraded = card.IsUpgraded,
        //        UpgradeCounter = card.UpgradeCounter
        //    };
        //    return Card;
        //}

        //public static List<CardObj> GetCards(IEnumerable<Card> cards)
        //{
        //    return cards.Select(card => GetCard(card)).ToList();
        //}

        //public static CardWithPrice GetCardWithPrice(Card card, int? price = null)
        //{
        //    CardWithPrice Card = new CardWithPrice
        //    {
        //        Id = card.Id,
        //        IsUpgraded = card.IsUpgraded,
        //        UpgradeCounter = card.UpgradeCounter,
        //        Price = price
        //    };
        //    return Card;
        //}

        internal static string GetBaseMana(string oldBaseMana, IEnumerable<string> exhibits)
        {
            string newBaseMana = oldBaseMana;
            foreach (string exhibit in exhibits)
            {
                ExhibitConfig config = ExhibitConfig.FromId(exhibit);
                Rarity rarity = config.Rarity;
                if (rarity != Rarity.Shining) continue;
                ManaColor? manaColor = config.BaseManaColor;
                if (manaColor == null) newBaseMana += "A";
            }
            return newBaseMana;
        }

        //public static string GetEnemyGroupId(Station station)
        //{
        //    EnemyGroup enemyGroup;
        //    if (station is BattleStation battleStation)
        //    {
        //        enemyGroup = battleStation.EnemyGroup;
        //        return enemyGroup.Id;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static string GetAdventureId(Station station)
        //{
        //    Adventure adventure;
        //    if (station is AdventureStation adventureStation)
        //    {
        //        adventure = adventureStation.Adventure;
        //        return adventure.Id;
        //    }
        //    else if (station is EntryStation entryStation)
        //    {
        //        adventure = entryStation.DebutAdventure;
        //        if (adventure == null) return null;
        //        return adventure.Id;
        //    }
        //    else if (station is TradeStation tradeStation)
        //    {
        //        adventure = tradeStation.Adventure;
        //        return adventure.Id;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public static List<T> GetList<T>(DialogStorage storage, IEnumerable<int> keys, string prefix, string suffix = "")
        //{
        //    List<T> ids = new List<T>();
        //    foreach (int key in keys)
        //    {
        //        string _key = $"{prefix}{key}{suffix}";
        //        storage.TryGetValue(_key, out object id);
        //        ids.Add((T)id);
        //    }
        //    return ids;
        //}

        //public static void Reset()
        //{
        //    InteractionViewerPatch.Listener = null;
        //    StationPatch.RewardListener = null;
        //    StationPatch.AddRewardsPatch.isAfterAddRewards = false;
        //    GameRunControllerPatch.isAfterBossReward = false;
        //}
    }
}