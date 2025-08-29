using HarmonyLib;
using LBoL.Core;
using RunLogger.Utils.RunLogLib;
using RunLogger.Utils;
using RunLogger.Utils.LogFile;

namespace RunLogger.Patches.SaveData
{
    [HarmonyPatch]
    internal static class LoadTemp
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.Restore)), HarmonyPrefix]
        private static void RestoreRun()
        {
            BepinexPlugin.log.LogDebug("Trying to restore run...");
            RunLog runLog = Logger.LoadTemp();
            if (runLog == null)
            {
                BepinexPlugin.log.LogDebug("Run restore failed");
                Controller.DestroyInstance();
                return;
            }
            Controller.CreateInstance(runLog);
            BepinexPlugin.log.LogDebug("Run restored");
        }
    }
}