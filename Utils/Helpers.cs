using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using Newtonsoft.Json;
using RunLogger.Utils.RunLogLib;
using System.Collections.Generic;

namespace RunLogger.Utils
{
    internal static class Helpers
    {
        internal static void AddStatus(GameRunController gameRun, StationObj stationObj, int? overrideHp)
        {
            PlayerUnit character = gameRun.Player;
            int hp = overrideHp ?? character.Hp;

            Status status = new Status
            {
                Money = gameRun.Money,
                Hp = hp,
                MaxHp = character.MaxHp,
                Power = character.Power,
                MaxPower = character.MaxPower
            };

            if (stationObj == null) Controller.Instance.RunLog.Settings.Status = status;
            else stationObj.Status = status;
        }

        internal static string GetEnemyGroupId(Station station)
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

        internal static string GetAdventureId(Station station)
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

        public static void GetData(out Dictionary<string, object> Data)
        {
            Data = Controller.CurrentStation.Data;
            if (Data == null) Data = Controller.CurrentStation.Data = new Dictionary<string, object>();
        }

        public static void AddDataValue<T>(string key, T value)
        {
            GetData(out Dictionary<string, object> Data);
            Data.Add(key, value);
        }

        public static void AddDataListItem<T>(string key, T item)
        {
            GetData(out Dictionary<string, object> Data);
            List<T> list = Data.TryGetValue(key, out object value) ? (List<T>)value : new List<T>();
            list.Add(item);
            Data[key] = list;
        }
    }
}