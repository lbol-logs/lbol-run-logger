using RunLogger.Utils.RunLogLib;
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

        internal static StationObj LastStation
        {
            get
            {
                int lastStationIndex = Controller.CurrentStationIndex - 1;
                if (lastStationIndex == -1) return null;
                StationObj station = Controller.Instance.RunLog.Stations[lastStationIndex];
                return station;
            }
        }

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

        internal static bool ShowRandomResult
        {
            get
            {
                bool showRandomResult = Controller.Instance.RunLog.Settings.ShowRandomResult;
                return showRandomResult;
            }
        }

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

        //public static void Reset()
        //{
        //    InteractionViewerPatch.Listener = null;
        //    StationPatch.RewardListener = null;
        //    StationPatch.AddRewardsPatch.isAfterAddRewards = false;
        //    GameRunControllerPatch.isAfterBossReward = false;
        //}
    }
}