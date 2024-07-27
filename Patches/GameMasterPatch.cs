using HarmonyLib;
using LBoL.Core.SaveData;
using LBoL.Core.Units;
using LBoL.Presentation;
using RunLogger.Utils;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameMaster))]
    class GameMasterPatch
    {
        [HarmonyPatch(nameof(GameMaster.AppendGameRunHistory)), HarmonyPostfix]
        static void AppendGameRunHistoryPatch(GameRunRecordSaveData record)
        {
            Debugger.Write("AppendGameRunHistory");
            string Result = record.ResultType.ToString();
            Debugger.Write("result: " + Result);
            string Timestamp = record.SaveTimestamp;
            Debugger.Write("Timestamp: " + Timestamp);
            RunDataController.RunData.Result = Result;
            RunDataController.RunData.Timestamp = Timestamp;
            RunDataController.Save();

            string ts = Timestamp.Replace(":", "-");
            string character = record.Player;
            string type = record.PlayerType.ToString();

            string name = $"{ts}_{character}_{type}_{Result}";
            RunDataController.Copy(name);

        }
        //{
        //    Debugger.Write("BattleStationFlow");
        //    int Turns = __instance.CurrentGameRun.Battle.RoundCounter;
        //    Debugger.Write("turns: " + Turns);
        //    RunDataController.AddData("Turns", Turns);
        //}

            //[HarmonyPatch(nameof(GameMaster.EndStationFlow)), HarmonyPostfix]
            //static void EndStationFlowPatch(GameMaster __instance)
            //{
            //    Debugger.Write("EndStationFlow");
            //    int Rounds = __instance.CurrentGameRun.Battle.RoundCounter;
            //    Debugger.Write("rounds: " + Rounds);
            //}
    }
}
