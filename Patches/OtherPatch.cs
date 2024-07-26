using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Stations;
using RunLogger.Utils;
using System;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    class OtherPatch
    {
        //[HarmonyPatch(typeof(GameMap), nameof(GameMap.CreateNormalMap)), HarmonyPostfix]
        //static void CreateNormalMapPatch(string bossId, bool isSelectingBoss, GameMap __result)
        //{
        //    for (int y = 0; y < 17; y++)
        //    {
        //        for (int x = 0; x < 5; x++)
        //        {
        //            MapNode mapNode = __result.Nodes[x, y];
        //            Node Node = new Node
        //            {
        //                X = mapNode.X,
        //                Y = mapNode.Y,
        //                Followers = mapNode.FollowerList
        //            };
        //        }
        //    }
        //    StageObj StageObj = new StageObj
        //    {
        //        Stage = int,
        //        Nodes = Nodes,
        //        Boss = bossId
        //    };
        //    RunDataController.RunData.Stages.Add(StageObj);
        //    RunDataController.Save();
        //}
    }
}
