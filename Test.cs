using HarmonyLib;
using LBoL.Core;
using LBoL.Core.SaveData;
using RunLogger.Utils;

namespace RunLogger
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(GameRunController))]
    class TestPatch
    {
        //[HarmonyPatch(nameof(GameRunController.Create)), HarmonyPostfix]
        //static void CreatePatch(GameRunController __result)
        //{
        //    bool HasClearBonus = __result.HasClearBonus;
        //    Debugger._Write("HasClearBonus: " + HasClearBonus);
        //}

        //[HarmonyPatch(nameof(GameRunController.Save)), HarmonyPostfix]
        //static void SavePatch(GameRunSaveData __result)
        //{
        //    Debugger._Write("saved");
        //    Debugger._Write($"name: {__result.PlayerType}");
        //}

        //[HarmonyPatch(nameof(GameRunController.Restore)), HarmonyPostfix]
        //static void RestorePatch(GameRunSaveData data, GameRunController __result)
        //{
        //    Debugger._Write("loaded");
        //    Debugger._Write($"timing: {data.Timing}");
        //    Debugger._Write($"name: {__result.Player.ModelName}");
        //}
    }
}
