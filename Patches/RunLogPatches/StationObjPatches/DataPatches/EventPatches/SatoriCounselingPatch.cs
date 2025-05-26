using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.EntityLib.PlayerUnits;
using RunLogger.Utils;
using RunLogger.Utils.Managers;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class SatoriCounselingPatch
    {
        [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.AdventureRand)), HarmonyPostfix]
        private static void AddHasMoney(int a, int __result, GameRunController ____gameRun)
        {
            if (a != 50) return;
            GameRunController gameRun = ____gameRun;
            if (gameRun.Player is Koishi) return;
            if (Helpers.GetAdventureId(gameRun.CurrentStation) != nameof(SatoriCounseling)) return;

            int money = gameRun.Money;
            bool hasMoney = money >= __result;
            Helpers.AddDataValue("HasMoney", hasMoney);
        }

        [HarmonyPatch(typeof(InteractionViewer), nameof(InteractionViewer.View)), HarmonyPrefix]
        private static void AddCardsRewards(Interaction interaction)
        {
            if (!Helpers.IsAdventure<SatoriCounseling>()) return;
            RewardsManager.AddCardsRewards(interaction);
        }
    }
}