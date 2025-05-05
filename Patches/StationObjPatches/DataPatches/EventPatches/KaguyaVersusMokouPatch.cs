using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage1;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class KaguyaVersusMokouPatch
    {
        [HarmonyPatch(typeof(KaguyaVersusMokou), nameof(KaguyaVersusMokou.InitVariables)), HarmonyPostfix]
        private static void AddHps(KaguyaVersusMokou __instance)
        {
            DialogStorage storage = __instance.Storage;
            List<int> hps = Helpers.GetStorageList<float, string>(storage, new[] { "", "Low" }, "$hpLose").Select(hp => (int)hp).ToList();
            Helpers.AddDataValue("Hps", hps);
        }
    }
}