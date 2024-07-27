using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch]
    class AdventurePatch
    {
        [HarmonyPatch(typeof(Debut))]
        class DebutPatch
        {
            [HarmonyPatch(nameof(Debut.RollBonus))]
            static void Postfix(Exhibit ____exhibit, int[] ____bonusNos)
            {
                if (!RunDataController.RunData.Settings.HasClearBonus) return;
                Exhibit _exhibit = ____exhibit;
                int[] _bonusNos = ____bonusNos;
                int Choice = 0;
                StationObj station = RunDataController.CurrentStation;
                station.Data = new Dictionary<string, object>
                {
                    { "Options", _bonusNos },
                    { "Choice", Choice }
                };
                if (RunDataController.ShowRandom) station.Data.Add("Shinning", _exhibit.Id);
            }
        }
    }
}
