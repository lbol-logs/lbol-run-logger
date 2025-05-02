using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Dialogs;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.EntityLib.PlayerUnits;
using RunLogger.Legacy.Patches;
using RunLogger.Legacy.Utils;
using RunLogger.Utils;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class SatoriCounselingPatch
    {
        [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.AdventureRand)), HarmonyPostfix]
        private static void AddHasMoney(int a, int __result, GameRunController ____gameRun)
        {
            if (a != Configs.SatoriCounselingMinimumPrice) return;
            GameRunController gameRun = ____gameRun;
            if (gameRun.Player is Koishi) return;
            if (Helpers.GetAdventureId(gameRun.CurrentStation) != nameof(SatoriCounseling)) return;

            int money = gameRun.Money;
            bool hasMoney = money >= __result;
            Helpers.AddDataValue("HasMoney", hasMoney);
            BepinexPlugin.log.LogDebug($"money: {money}, price: {__result}, hasMoney: {hasMoney}");
        }

        //[HarmonyPatch(typeof(SatoriCounseling), nameof(SatoriCounseling.Library)), HarmonyPostfix]
        //public static void LibraryPatch()
        //{
        //    InteractionViewerPatch.Listener = nameof(SatoriCounseling);
        //    isMini = false;
        //}

        //[HarmonyPatch(typeof(SatoriCounseling), nameof(SatoriCounseling.Analyse)), HarmonyPostfix]
        //public static void AnalysePatch()
        //{
        //    InteractionViewerPatch.Listener = nameof(SatoriCounseling);
        //    isMini = true;
        //}
    }
}