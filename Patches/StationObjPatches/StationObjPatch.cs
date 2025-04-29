using HarmonyLib;
using LBoL.Core.Stations;
using LBoL.Core;
using RunLogger.Utils.RunLogLib.Nodes;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils;

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

            string id = Helpers.GetAdventureId(currentStation);
            if (id != null)
            {
                station.Id = id;
            }
            else
            {
                id = Helpers.GetEnemyGroupId(currentStation);
                station.Id = id;
            }
            Controller.Instance.RunLog.Stations.Add(station);
        }
    }
}
