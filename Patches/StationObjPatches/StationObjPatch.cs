using HarmonyLib;
using LBoL.Core.Stations;
using LBoL.Core;
using RunLogger.Utils.RunLogLib.Nodes;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils;
using LBoL.Core.Adventures;
using LBoL.Core.Units;
using RunLogger.Utils.RunLogLib.Entities;

namespace RunLogger.Patches.StationObjPatches
{
    [HarmonyPatch]
    public static class StationObjPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterMapNode)), HarmonyPostfix]
        static void NewStation(MapNode node, GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            int act = gameRun.CurrentStage.Level;
            Station currentStation = gameRun.CurrentStation;
            int level = currentStation.Level;
            int y = node.Y;
            string type = node.StationType.ToString();

            StationNode stationNode = new StationNode
            {
                Act = act,
                Level = level,
                Y = y
            };
            StationObj station = new StationObj
            {
                Type = type,
                Node = stationNode
            };

            string id = StationObjPatch.GetAdventureId(currentStation);
            if (id != null)
            {
                station.Id = id;
            }
            else
            {
                id = StationObjPatch.GetEnemyGroupId(currentStation);
                station.Id = id;
            }
            Controller.Instance.RunLog.Stations.Add(station);

            StationObjPatch.AddStartingEntities();
        }

        private static string GetEnemyGroupId(Station station)
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

        private static string GetAdventureId(Station station)
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

        private static void AddStartingEntities()
        {
            if (Controller.Instance.IsOverridingStartingDeck)
            {
                EntitiesManager.AddCardChange(Controller.Instance.StartingDeckOverride, ChangeType.Add);
                Controller.Instance.IsOverridingStartingDeck = false;
                Controller.Instance.StartingDeckOverride = null;
            }

            //if (startingCards != null)
            //{
            //    RunDataController.AddCardChange(startingCards, ChangeType.Add);
            //    startingCards = null;
            //}

            //if (startingExhibits.Any())
            //{
            //    foreach (Exhibit exhibit in startingExhibits)
            //    {
            //        RunDataController.AddExhibitChange(exhibit, ChangeType.Add);
            //    }
            //    startingExhibits.Clear();
            //}
        }
    }
}
