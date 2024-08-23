using HarmonyLib;
using LBoL.Core;
using LBoL.EntityLib.Adventures;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch]
    public static class AdventurePatch
    {
        [HarmonyPatch(typeof(Debut))]
        public static class DebutPatch
        {
            [HarmonyPatch(nameof(Debut.RollBonus))]
            public static void Postfix(Exhibit ____exhibit, int[] ____bonusNos, Debut __instance)
            {
                if (!RunDataController.RunData.Settings.HasClearBonus) return;
                Exhibit _exhibit = ____exhibit;
                int[] _bonusNos = ____bonusNos;
                RunDataController.AddData("Options", _bonusNos);

                if (RunDataController.ShowRandom)
                {
                    RunDataController.AddData("Shinning", _exhibit.Id);
                    foreach (int _bonusNo in _bonusNos)
                    {
                        switch (_bonusNo)
                        {
                            case 0:
                                List<string> uncommonCards = GetUncommonCards(__instance);
                                RunDataController.AddData("UncommonCards", uncommonCards);
                                break;
                            case 1:
                                __instance.Storage.TryGetValue("$rareCard", out string rareCard);
                                RunDataController.AddData("RareCard", rareCard);
                                break;
                            case 2:
                                __instance.Storage.TryGetValue("$rareExhibit", out string rareExhibit);
                                RunDataController.AddData("RareExhibit", rareExhibit);
                                break;
                            case 5:
                                __instance.Storage.TryGetValue("$transformCard", out string transformCard);
                                RunDataController.AddData("TransformCard", transformCard);
                                break;
                        }
                    }
                }
            }

            private static List<string> GetUncommonCards(Debut debut)
            {
                debut.Storage.TryGetValue("$uncommonCard1", out string uncommonCard1);
                debut.Storage.TryGetValue("$uncommonCard2", out string uncommonCard2);
                debut.Storage.TryGetValue("$uncommonCard3", out string uncommonCard3);
                List<string> uncommonCards = new List<string> { uncommonCard1, uncommonCard2, uncommonCard3 };
                return uncommonCards;
            }
        }
    }
}
