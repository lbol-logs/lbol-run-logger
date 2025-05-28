using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Dialogs;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class MiyoiBartenderPatch
    {
        [HarmonyPatch(typeof(MiyoiBartender), nameof(MiyoiBartender.InitVariables)), HarmonyPrefix]
        private static void AddIds(MiyoiBartender __instance)
        {
            if (!Instance.IsInitialized) return;

            UniqueRandomPool<string> uniqueRandomPool = __instance.Stage.EnemyPoolAct3;
            List<string> ids = uniqueRandomPool.Select(e => e.Elem).ToList();
            Helpers.AddDataValue("Ids", ids);
        }

        [HarmonyPatch(typeof(MiyoiBartender), nameof(MiyoiBartender.InitVariables)), HarmonyPostfix]
        private static void AddExhibitBeforeBattle(MiyoiBartender __instance)
        {
            if (!Instance.IsInitialized) return;

            if (!Controller.ShowRandomResult) return;
            MiyoiBartenderPatch.AddExhibit(__instance.Storage);
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.LeaveBattle)), HarmonyPostfix]
        private static void AddExhibitAfterBattle(GameRunController __instance)
        {
            if (!Instance.IsInitialized) return;

            if (Controller.ShowRandomResult) return;
            if (!Helpers.IsAdventure<MiyoiBartender>(out DialogStorage storage)) return;
            MiyoiBartenderPatch.AddExhibit(storage);
        }

        private static void AddExhibit(DialogStorage storage)
        {
            storage.TryGetValue("$randomExhibit", out string exhibit);
            Helpers.AddDataValue("Exhibit", exhibit);
        }
    }
}