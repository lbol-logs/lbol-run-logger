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
            RunDataController.Restore();
            string Result = record.ResultType.ToString();
            string Timestamp = record.SaveTimestamp;
            RunDataController.RunData.Result = Result;
            RunDataController.RunData.Timestamp = Timestamp;
            RunDataController.Save();

            string ts = Timestamp.Replace(":", "-");
            string character = record.Player;
            string type = record.PlayerType.ToString();

            string name = $"{ts}_{character}_{type}_{Result}";
            RunDataController.Copy(name);

        }
    }
}
