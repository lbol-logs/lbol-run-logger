using HarmonyLib;
using LBoL.Core.SaveData;
using LBoL.Presentation;
using RunLogger.Utils;
using RunLogger.Utils.Enums;
using RunLogger.Utils.LogFile;

namespace RunLogger.Patches.SaveData
{
    [HarmonyPatch]
    internal static class SaveTemp
    {
        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.SaveGameRun)), HarmonyPrefix]
        private static void SaveTempInAdventure(GameRunSaveData data, bool normalSave)
        {
            bool toSave = Instance.IsInitialized && data.Timing == SaveTiming.Adventure && normalSave;
            if (!toSave) return;
            Logger.SaveTemp(TempSaveTiming.Adventure);
        }
    }
}