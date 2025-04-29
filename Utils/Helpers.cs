using LBoL.Core.Adventures;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using System.Collections.Generic;

namespace RunLogger.Utils
{
    internal static class Helpers
    {
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
    }
}