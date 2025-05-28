using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage2;
using RunLogger.Utils;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class BuduSuanmingPatch
    {
        [HarmonyPatch(typeof(DialogRunner), nameof(DialogRunner.SelectOption)), HarmonyPostfix]
        private static void AddData(int id)
        {
            if (!Instance.IsInitialized) return;

            if (!Helpers.IsAdventure<BuduSuanming>(out DialogStorage storage)) return;
            if (id == 0)
            {
                storage.TryGetValue("$bossId", out string boss);
                Helpers.AddDataValue("Boss", boss);
            }
            else if (id == 1)
            {
                storage.TryGetValue("$hostId", out string host);
                Helpers.AddDataValue("Host", host);
            }
            else if (id == 2)
            {
                storage.TryGetValue("$seijaItem", out string exhibit);
                Helpers.AddDataValue("Exhibit", exhibit);
            }
        }
    }
}