using HarmonyLib;
using LBoL.Presentation;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameMaster))]
    class GameMasterPatch
    {
        //[HarmonyPatch(nameof(GameMaster.StartGame)), HarmonyPrefix]
        //static void StartGamePatch(Debut __instance)
        //{
        //    int[] _bonusNos = __instance._bonusNos;
        //    string[] _optionTitles = __instance._optionTitles;
        //    Debugger._Write("_Write #3: " + _optionTitles[_bonusNos[0]]);
        //    Debugger._Write("_Write #4: " + _optionTitles[_bonusNos[1]]);
        //}
    }
}
