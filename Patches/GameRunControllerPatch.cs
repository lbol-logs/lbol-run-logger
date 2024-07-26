using HarmonyLib;
using LBoL.Core;
using Newtonsoft.Json;
using RunLogger.Utils;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameRunController))]
    class GameRunControllerPatch
    {
        [HarmonyPatch(nameof(GameRunController.Create)), HarmonyPostfix]
        static void CreatePatch(GameRunStartupParameters parameters)
        {
            Debugger.Write("created");
            string PlayerType = parameters.PlayerType.ToString();
            bool HasClearBonus = parameters.UserProfile.HasClearBonus;
            bool ShowRandomResult = parameters.ShowRandomResult;
            RunDataController.Create();
            RunDataController.RunData.Info.PlayerType = PlayerType;
            RunDataController.RunData.Info.ShowRandomResult = ShowRandomResult;
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.Save)), HarmonyPostfix]
        static void SavePatch()
        {
            Debugger.Write("saved");
            RunDataController.Save();
        }

        [HarmonyPatch(nameof(GameRunController.Restore)), HarmonyPostfix]
        static void RestorePatch()
        {
            Debugger.Write("loaded");
            RunDataController.Restore();
        }

        [HarmonyPatch(nameof(GameRunController.EnterMapNode)), HarmonyPostfix]
        static void EnterMapNodePatch(MapNode node, GameRunController __instance)
        {
            int Stage = __instance.CurrentStage.Level;
            int Level = __instance.CurrentStation.Level;
            string Type = node.StationType.ToString();
            int X = node.X;
            int Y = node.Y;
            Debugger.Write($"x: {X}, y: {Y}");
            Station station = new Station
            {
                Stage = Stage,
                Level = Level,
                Type = Type,
                X = X,
                Y = Y
            };
            RunDataController.RunData.Stations.Add(station);
            Debugger.Write($"station: {JsonConvert.SerializeObject(station)}");
        }
    }
}
