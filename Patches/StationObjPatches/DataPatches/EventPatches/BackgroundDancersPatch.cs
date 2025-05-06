using HarmonyLib;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures.Stage3;
using RunLogger.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class BackgroundDancersPatch
    {
        [HarmonyPatch(typeof(BackgroundDancers), nameof(BackgroundDancers.InitVariables)), HarmonyPostfix]
        private static void AddHp(BackgroundDancers __instance)
        {
            __instance.Storage.TryGetValue("$hpLose", out float hp);
            Helpers.AddDataValue("Hp", (int)hp);
        }

        [HarmonyPatch(typeof(BackgroundDancers), nameof(BackgroundDancers.RollOptions)), HarmonyPostfix]
        private static void AddOptionsAndData(BackgroundDancers __instance, int[] ____optionIndices)
        {
            Helpers.AddDataValue("Options", ____optionIndices.ToList());
            foreach (int option in ____optionIndices) BackgroundDancersPatch.HandleRandom(__instance, option);
        }

        [HarmonyPatch(typeof(BackgroundDancers), nameof(BackgroundDancers.SelectOption)), HarmonyPostfix]
        private static void AddData(int index, ref IEnumerator __result)
        {
            static void postfixAction()
            {
                BackgroundDancers backgroundDancers = Helpers.GetAdventure(null) as BackgroundDancers;
                int option = backgroundDancers._optionIndices[(int)Controller.Instance.BackgroundDancersIndex];
                Controller.Instance.BackgroundDancersIndex = null;
                Helpers.AddDataListItem("Options", option);
                BackgroundDancersPatch.HandleRandom(backgroundDancers, option);
            }

            EnumeratorHook enumeratorHook = new EnumeratorHook()
            {
                enumerator = __result,
                postfixAction = postfixAction
            };

            Controller.Instance.BackgroundDancersIndex = index - 1;
            __result = enumeratorHook.GetEnumerator();
        }

        private static void HandleRandom(BackgroundDancers __instance, int option)
        {
            if (!Controller.ShowRandomResult) return;
            DialogStorage storage = __instance.Storage;
            switch (option)
            {
                case 0:
                case 2:
                case 3:
                    break;
                case 1:
                    List<string> tools = Helpers.GetStorageList<string, string>(storage, new[] { "", "2" }, "$reward2Rare");
                    Helpers.AddDataListItem("Tools", tools);
                    break;
                case 4:
                    storage.TryGetValue("$reward5Exhibit", out string exhibit);
                    Helpers.AddDataListItem("Exhibits", exhibit);
                    break;
                case 5:
                    storage.TryGetValue("$reward6Ability", out string ability);
                    Helpers.AddDataListItem("Abilities", ability);
                    break;
            }
        }
    }
}