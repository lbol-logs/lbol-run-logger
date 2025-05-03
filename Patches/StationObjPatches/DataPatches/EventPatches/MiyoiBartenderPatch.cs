using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class MiyoiBartenderPatch
    {
        [HarmonyPatch(typeof(MiyoiBartender), nameof(MiyoiBartender.InitVariables)), HarmonyPrefix]
        private static void AddIds(MiyoiBartender __instance)
        {
            UniqueRandomPool<string> uniqueRandomPool = __instance.Stage.EnemyPoolAct3;
            List<string> ids = uniqueRandomPool.Select((e) => e.Elem).ToList();
            Helpers.AddDataValue("Ids", ids);
        }

        [HarmonyPatch(typeof(MiyoiBartender), nameof(MiyoiBartender.InitVariables)), HarmonyPostfix]
        private static void AddExhibit(MiyoiBartender __instance)
        {
            __instance.Storage.TryGetValue("$randomExhibit", out string exhibit);
            if (Controller.ShowRandomResult) Helpers.AddDataValue("Exhibit", exhibit);
            else Controller.Instance.MiyoiBartenderExhibit = exhibit;
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddExhibitAfterBattle(GameRunController __instance)
        {
            GameRunController gameRun = __instance;
            string adventureId = Helpers.GetAdventureId(gameRun.CurrentStation);
            if (adventureId != nameof(MiyoiBartender)) return;
            if (Controller.ShowRandomResult) return;
            Helpers.AddDataValue("Exhibit", Controller.Instance.MiyoiBartenderExhibit);
            Controller.Instance.MiyoiBartenderExhibit = null;
        }
    }
}