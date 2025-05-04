using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage1;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class KaguyaVersusMokouPatch
    {
        [HarmonyPatch(typeof(KaguyaVersusMokou), nameof(KaguyaVersusMokou.InitVariables)), HarmonyPostfix]
        private static void AddHps(KaguyaVersusMokou __instance)
        {
            DialogStorage storage = __instance.Storage;
            storage.TryGetValue("$hpLose", out float hpLose);
            storage.TryGetValue("$hpLoseLow", out float hpLoseLow);
            Helpers.AddDataValue("Hps", new[] { (int)hpLose, (int)hpLoseLow });
        }
    }
}