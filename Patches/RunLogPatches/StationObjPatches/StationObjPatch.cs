using HarmonyLib;
using LBoL.Core.Stations;
using LBoL.Core;
using RunLogger.Utils.RunLogLib.Nodes;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils;
using LBoL.Core.Units;
using LBoL.Core.Adventures;
using System.Collections.Generic;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches
{
    [HarmonyPatch]
    internal static class StationObjPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterMapNode)), HarmonyPrefix]
        private static void GetForced(bool forced)
        {
            Controller.Instance.IsForced = forced;
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterStation)), HarmonyPostfix]
        private static void NewStation(Station station, GameRunController __instance)
        {
            bool? forced = Controller.Instance.IsForced;
            Controller.Instance.IsForced = null;
            if (forced == true) return;

            GameRunController gameRun = __instance;
            int act = station.Stage.Level;
            int level = station.Level;
            MapNode node = gameRun.CurrentMap.VisitingNode;
            int y = node.Y;
            string type = node.StationType.ToString();

            StationNode stationNode = new StationNode
            {
                Act = act,
                Level = level,
                Y = y
            };
            StationObj stationObj = new StationObj
            {
                Type = type,
                Node = stationNode
            };

            string id = Helpers.GetAdventureId(station);
            if (id != null)
            {
                stationObj.Id = id;
            }
            else
            {
                id = Helpers.GetEnemyGroupId(station);
                stationObj.Id = id;
            }
            Controller.Instance.RunLog.Stations.Add(stationObj);

            List<IEnumerable<StationReward>> rewardsBeforeDebut = Controller.Instance.RewardsBeforeDebut;
            if (rewardsBeforeDebut == null) return;
            foreach (IEnumerable<StationReward> rewards in rewardsBeforeDebut) RewardsManager.AddRewards(rewards);
            Controller.Instance.RewardsBeforeDebut = null;
        }

        [HarmonyPatch(typeof(BattleAdvTestStation), nameof(BattleAdvTestStation.SetEnemy)), HarmonyPostfix]
        private static void AddEnemyGroupId(EnemyGroupEntry entry)
        {
            Controller.CurrentStation.Id = entry.Id;
        }

        [HarmonyPatch(typeof(BattleAdvTestStation), nameof(BattleAdvTestStation.SetAdventure)), HarmonyPostfix]
        private static void AddAdventureId(Adventure adventure)
        {
            Controller.CurrentStation.Id = adventure.Id;
        }
    }
}