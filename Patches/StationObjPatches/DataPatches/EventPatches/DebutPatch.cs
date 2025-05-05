using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Dialogs;
using LBoL.EntityLib.Adventures;
using RunLogger.Utils;
using System;
using System.Collections.Generic;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class DebutPatch
    {
        [HarmonyPatch(typeof(Debut), nameof(Debut.RollBonus)), HarmonyPostfix]
        private static void AddData(Exhibit ____exhibit, int[] ____bonusNos, Debut __instance)
        {
            if (!Controller.Instance.RunLog.Settings.HasClearBonus) return;

            int[] options = ____bonusNos;
            Helpers.AddDataValue("Options", options);

            int i = Array.IndexOf(options, 0);
            if (i != -1) Controller.Instance.DebutUncommonCardsIndex = i + 2;

            if (!Controller.ShowRandomResult) return;

            Helpers.AddDataValue("Shining", ____exhibit.Id);

            DialogStorage storage = __instance.Storage;
            foreach (int bonusNo in options)
            {
                switch (bonusNo)
                {
                    case 0:
                        List<string> uncommonCards = Helpers.GetStorageList<string, int>(storage, new[] { 1, 2, 3 }, "$uncommonCard");
                        Helpers.AddDataValue("UncommonCards", uncommonCards);
                        break;
                    case 1:
                        storage.TryGetValue("$rareCard", out string rareCard);
                        Helpers.AddDataValue("RareCard", rareCard);
                        break;
                    case 2:
                        storage.TryGetValue("$rareExhibit", out string rareExhibit);
                        Helpers.AddDataValue("RareExhibit", rareExhibit);
                        break;
                    case 5:
                        storage.TryGetValue("$transformCard", out string transformCard);
                        Helpers.AddDataValue("TransformCard", transformCard);
                        break;
                }
            }
        }

        [HarmonyPatch(typeof(DialogRunner), nameof(DialogRunner.SelectOption)), HarmonyPostfix]
        private static void Prefix(int id)
        {
            if (id != Controller.Instance.DebutUncommonCardsIndex) return;
            Controller.Instance.DebutUncommonCardsChosen = true;
        }

        [HarmonyPatch(typeof(InteractionViewer), nameof(InteractionViewer.View)), HarmonyPrefix]
        private static void AddCardsRewards(Interaction interaction)
        {
            if (!Controller.Instance.DebutUncommonCardsChosen) return;
            if (!Helpers.IsAdventure<Debut>()) return;
            RewardsManager.AddCardsRewards(interaction);
            Controller.Instance.DebutUncommonCardsIndex = null;
            Controller.Instance.DebutUncommonCardsChosen = false;
        }
    }
}