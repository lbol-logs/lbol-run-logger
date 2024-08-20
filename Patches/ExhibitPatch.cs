using HarmonyLib;
using LBoL.Core;
using RunLogger.Utils;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Exhibit))]
    class ExhibitPatch
    {
        [HarmonyPatch("OnGain"), HarmonyPostfix]
        static void OnGainPatch(Exhibit __instance)
        {
            string exhibit = __instance.Id;
            switch (exhibit)
            {
                case "Modaoshu":
                    RunDataController.isListening = true;
                    RunDataController.AddData(exhibit, RunDataController.Cards);
                    RunDataController.Cards = null;
                    break;
            }
        }
    }
}
