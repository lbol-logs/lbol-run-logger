using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Stations;
using Newtonsoft.Json;
using RunLogger.Utils;
using System;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameRunController))]
    class GameRunControllerPatch
    {
        [HarmonyPatch(nameof(GameRunController.Create)), HarmonyPostfix]
        static void CreatePatch(GameRunStartupParameters parameters)
        {
            string Character = parameters.Player.ModelName;
            string PlayerType = parameters.PlayerType.ToString();
            bool HasClearBonus = parameters.UserProfile.HasClearBonus;
            bool ShowRandomResult = parameters.ShowRandomResult;
            bool IsAutoSeed = parameters.Seed == null;
            RunDataController.Create();
            RunDataController.RunData.Info.Character = Character;
            RunDataController.RunData.Info.PlayerType = PlayerType;
            RunDataController.RunData.Info.ShowRandomResult = ShowRandomResult;
            RunDataController.RunData.Info.IsAutoSeed = IsAutoSeed;
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.Save)), HarmonyPostfix]
        static void SavePatch()
        {
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.Restore)), HarmonyPostfix]
        static void RestorePatch()
        {
            RunDataController.Restore();
        }

        [HarmonyPatch(nameof(GameRunController.EnterStage)), HarmonyPostfix]
        static void EnterStagePatch(GameRunController __instance)
        {
            int Stage = __instance.CurrentStage.Level;
            GameMap gameMap = __instance.CurrentMap;
            string bossId = gameMap.BossId;
            List<Node> Nodes = new List<Node>();
            for (int x = 0; x < 17; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    MapNode mapNode = gameMap.Nodes[x, y];
                    if (mapNode == null) continue;
                    Node Node = new Node
                    {
                        X = mapNode.X,
                        Y = mapNode.Y,
                        Followers = mapNode.FollowerList,
                        Type = mapNode.StationType.ToString()
                    };
                    Nodes.Add(Node);
                }
            }
            StageObj StageObj = new StageObj
            {
                Stage = Stage,
                Nodes = Nodes,
                Boss = bossId
            };
            RunDataController.RunData.Stages.Add(StageObj);
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.EnterMapNode)), HarmonyPostfix]
        static void EnterMapNodePatch(MapNode node, GameRunController __instance)
        {
            int Stage = __instance.CurrentStage.Level;
            LBoL.Core.Stations.Station CurrentStation = __instance.CurrentStation;
            int Level = CurrentStation.Level;
            string Type = node.StationType.ToString();
            int X = node.X;
            int Y = node.Y;
            Utils.Station station = new Utils.Station
            {
                Stage = Stage,
                Level = Level,
                Type = Type,
                X = X,
                Y = Y
            };
            if (CurrentStation is AdventureStation AdventureStation)
            {
                string Name = AdventureStation.Adventure.GetType().Name;
                station.Name = Name;
            }
            RunDataController.RunData.Stations.Add(station);
        }
    }
}
