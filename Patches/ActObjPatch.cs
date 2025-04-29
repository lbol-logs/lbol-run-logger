using HarmonyLib;
using LBoL.Core;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils.RunLogLib.Nodes;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyPatch]
    public static class ActObjPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterStage)), HarmonyPostfix]
        static void NewAct(GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            int act = gameRun.CurrentStage.Level;
            GameMap gameMap = gameRun.CurrentMap;
            string bossId = gameMap.BossId;
            List<ActNode> actNodes = new List<ActNode>();
            for (int x = 0; x < gameMap.Nodes.GetLength(0); x++)
            {
                for (int y = 0; y < gameMap.Nodes.GetLength(1); y++)
                {
                    MapNode mapNode = gameMap.Nodes[x, y];
                    if (mapNode == null) continue;
                    ActNode actNode = new ActNode
                    {
                        X = mapNode.X,
                        Y = mapNode.Y,
                        Followers = mapNode.FollowerList,
                        Type = mapNode.StationType.ToString()
                    };
                    actNodes.Add(actNode);
                }
            }
            ActObj actObj = new ActObj
            {
                Act = act,
                Nodes = actNodes,
                Boss = bossId
            };
            Controller.Instance.RunLog.Acts.Add(actObj);
            Logger.SaveTemp(TempSaveTiming.EnterStage);
        }
    }
}
