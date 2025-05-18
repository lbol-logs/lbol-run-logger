using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Stations;
using LBoL.Presentation;
using RunLogger.Utils;
using RunLogger.Utils.Enums;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils.RunLogLib.Nodes;
using System.Collections;
using System.Collections.Generic;

namespace RunLogger.Patches.RunLogPatches
{
    [HarmonyPatch]
    internal static class ActObjPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterStage)), HarmonyPostfix]
        private static void NewAct(GameRunController __instance)
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

        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SelectStationFlow)), HarmonyPostfix]
        private static void AddBoss(ref IEnumerator __result)
        {
            static void postfixAction()
            {
                SelectStation selectStation = Helpers.CurrentStation as SelectStation;
                string boss = selectStation.Stage.SelectedBoss;
                Controller.Instance.RunLog.Acts[0].Boss = boss;
            }

            EnumeratorHook enumeratorHook = new EnumeratorHook()
            {
                enumerator = __result,
                postfixAction = postfixAction
            };

            __result = enumeratorHook.GetEnumerator();
        }
    }
}