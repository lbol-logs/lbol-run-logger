using RunLogger.Utils.RunLogLib.BattleDetails;
using System.Collections.Generic;

namespace RunLogger.Utils
{
    internal static class TurnObjManager
    {
        internal static void GetLastTurnObj(out TurnObj turnObj)
        {
            List<TurnObj> details = Controller.CurrentStation.Data["Details"] as List<TurnObj>;
            turnObj = details[^1];
        }

        internal static void AppendTurnObj(int round, int turn, string id)
        {
            TurnObj turnObj = new TurnObj()
            {
                Round = round,
                Turn = turn,
                Id = id
            };
            Helpers.AddDataListItem("Details", turnObj);
        }
    }
}