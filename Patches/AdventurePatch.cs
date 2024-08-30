﻿using HarmonyLib;
using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Adventure))]
    public static class AdventurePatch
    {
        [HarmonyPatch("InitVariables")]
        public static void Postfix(Adventure __instance)
        {
            string Id = __instance.Id;
            string[] manaEvents = new[] { "JunkoColorless", "PatchouliPhilosophy" };
            if (manaEvents.Contains(Id))
            {
                GameRunController gameRun = __instance.GameRun;
                string[] exhibits = gameRun.ExhibitRecord.ToArray();
                string baseMana = RunDataController.GetBaseMana(gameRun.BaseMana.ToString(), exhibits);
                RunDataController.AddData("BaseMana", baseMana);
            }
        }

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

        [HarmonyPatch(typeof(Supply))]
        public static class SupplyPatch
        {
            [HarmonyPatch(nameof(Supply.InitVariables))]
            public static void Postfix(Supply __instance)
            {
                __instance.Storage.TryGetValue("$exhibitA", out string exhibitA);
                __instance.Storage.TryGetValue("$exhibitB", out string exhibitB);
                RunDataController.AddData("Exhibits", new List<string>() { exhibitA, exhibitB });

                __instance.Storage.TryGetValue("$bothFlag", out bool bothFlag);
                RunDataController.AddData("Both", bothFlag);
            }
        }

        [HarmonyPatch(typeof(RinnosukeTrade))]
        public static class RinnosukeTradePatch
        {
            [HarmonyPatch(nameof(RinnosukeTrade.InitVariables))]
            public static void Postfix(Supply __instance)
            {
                __instance.Storage.TryGetValue("$canSell1", out bool canSell1);
                __instance.Storage.TryGetValue("$canSell2", out bool canSell2);

                if (canSell1)
                {
                    Dictionary<string, int> prices = new Dictionary<string, int>();

                    __instance.Storage.TryGetValue("$exhibit1", out string exhibit1);
                    __instance.Storage.TryGetValue("$exhibit1Price", out float exhibit1Price);
                    prices.Add(exhibit1, (int)exhibit1Price);

                    if (canSell2)
                    {
                        __instance.Storage.TryGetValue("$exhibit2", out string exhibit2);
                        __instance.Storage.TryGetValue("$exhibit2Price", out float exhibit2Price);
                        prices.Add(exhibit2, (int)exhibit2Price);
                    }

                    RunDataController.AddData("Prices", prices);
                }
            }
        }

        [HarmonyPatch(typeof(DoremyPortal))]
        public static class DoremyPortalPatch
        {
            [HarmonyPatch(nameof(DoremyPortal.InitVariables))]
            public static void Postfix(DoremyPortal __instance)
            {
                if (RunDataController.ShowRandom)
                {
                    __instance.Storage.TryGetValue("$randomExhibit", out string randomExhibit);
                    RunDataController.AddData("Exhibit", randomExhibit);
                }
            }
        }

        [HarmonyPatch(typeof(SelectBaseManaInteraction), nameof(SelectBaseManaInteraction.SelectedMana), MethodType.Setter)]
        public static class SelectBaseManaInteractionPatch
        {
            static void Prefix(ManaGroup value)
            {
                ManaGroup mana = value;
                string color = mana.MaxColor.ToShortName().ToString();
                RunDataController.AddData("Color", color);
            }

            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return instructions;
            }
        }
    }
}
