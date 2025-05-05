using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Dialogs;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.Presentation;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;
using System.Linq;

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
            EnemyGroup enemyGroup = null;
            if (station is BattleAdvTestStation battleAdvTestStation) enemyGroup = battleAdvTestStation.EnemyGroup;
            else if (station is BattleStation battleStation) enemyGroup = battleStation.EnemyGroup;
            return enemyGroup?.Id;
        }

        internal static Adventure GetAdventure(Station station)
        {
            station ??= Singleton<GameMaster>.Instance.CurrentGameRun.CurrentStation;
            Adventure adventure = null;
            if (station is BattleAdvTestStation battleAdvTestStation) adventure = battleAdvTestStation.Adventure;
            else if (station is AdventureStation adventureStation) adventure = adventureStation.Adventure;
            else if (station is EntryStation entryStation) adventure = entryStation.DebutAdventure;
            else if (station is TradeStation tradeStation) adventure = tradeStation.Adventure;
            return adventure;
        }

        internal static string GetAdventureId(Station station)
        {
            return Helpers.GetAdventure(station)?.Id;
        }

        internal static bool IsAdventure<T>() where T : Adventure
        {
            return Helpers.IsAdventure<T>(out _);
        }

        internal static bool IsAdventure<T>(out DialogStorage storage) where T : Adventure
        {
            Adventure adventure = Helpers.GetAdventure(null);
            storage = adventure?.Storage;
            return adventure is T;
        }

        internal static CardObj ParseCard(Card card)
        {
            CardObj cardObj = new CardObj
            {
                Id = card.Id,
                IsUpgraded = card.IsUpgraded,
                UpgradeCounter = card.UpgradeCounter
            };
            return cardObj;
        }

        internal static List<CardObj> ParseCards(IEnumerable<Card> cards)
        {
            return cards.Select(card => Helpers.ParseCard(card)).ToList();
        }

        internal static string GetBaseMana(string oldBaseMana, IEnumerable<string> exhibits)
        {
            string newBaseMana = oldBaseMana;
            foreach (string exhibit in exhibits)
            {
                ExhibitConfig config = ExhibitConfig.FromId(exhibit);
                if (config.Rarity == Rarity.Shining && config.BaseManaColor == null) newBaseMana += Configs.RandomMana;
            }
            return newBaseMana;
        }

        internal static void GetRewards(out Dictionary<string, object> rewards)
        {
            rewards = Controller.CurrentStation.Rewards ??= new Dictionary<string, object>();
        }

        internal static void GetData(out Dictionary<string, object> data)
        {
            data = Controller.CurrentStation.Data ??= new Dictionary<string, object>();
        }

        internal static void AddDataValue<T>(string key, T value)
        {
            Helpers.GetData(out Dictionary<string, object> data);
            data.Add(key, value);
        }

        internal static void AddDataListItem<T>(string key, T item)
        {
            Helpers.GetData(out Dictionary<string, object> data);
            List<T> list = data.TryGetValue(key, out object value) ? (List<T>)value : new List<T>();
            list.Add(item);
            data[key] = list;
        }

        internal static void AddCardsRewards(Interaction interaction)
        {
            IReadOnlyList<Card> pendingCards;
            if (interaction is MiniSelectCardInteraction miniSelectCardInteraction) pendingCards = miniSelectCardInteraction.PendingCards;
            else pendingCards = (interaction as SelectCardInteraction).PendingCards;
            List<CardObj> cardObjs = Helpers.ParseCards(pendingCards);
            Helpers.GetRewards(out Dictionary<string, object> rewards);
            rewards.Add("Cards", new List<List<CardObj>>() { cardObjs });
        }

        internal static List<T1> GetStorageList<T1, T2>(DialogStorage storage, T2[] keys, string prefix, string suffix = "")
        {
            List<T1> values = new List<T1>();
            foreach (T2 key in keys)
            {
                string name = $"{prefix}{key}{suffix}";
                storage.TryGetValue(name, out object value);
                values.Add((T1)value);
            }
            return values;
        }
    }
}