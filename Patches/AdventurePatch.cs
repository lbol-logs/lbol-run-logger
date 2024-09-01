using HarmonyLib;
using LBoL.Base;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Dialogs;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
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

                if (!RunDataController.ShowRandom) return;

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

        [HarmonyPatch(typeof(MiyoiBartender))]
        public static class MiyoiBartenderPatch
        {
            [HarmonyPatch(nameof(MiyoiBartender.InitVariables))]
            public static void Prefix(MiyoiBartender __instance)
            {
                UniqueRandomPool<string> uniqueRandomPool = __instance.Stage.EnemyPoolAct3;
                List<string> ids = uniqueRandomPool.Select((RandomPoolEntry<string> e) => e.Elem).ToList();
                RunDataController.AddData("Ids", ids);
            }

            [HarmonyPatch(nameof(MiyoiBartender.InitVariables))]
            public static void Postfix(MiyoiBartender __instance)
            {
                if (RunDataController.ShowRandom)
                {
                    __instance.Storage.TryGetValue("$randomExhibit", out string randomExhibit);
                    RunDataController.AddData("Exhibit", randomExhibit);
                }
            }
        }

        [HarmonyPatch(typeof(SumirekoGathering))]
        public static class SumirekoGatheringPatch
        {
            private static List<string> rareCards;

            [HarmonyPatch(nameof(SumirekoGathering.InitVariables))]
            public static void Postfix(SumirekoGathering __instance)
            {
                rareCards = GetRareCards(__instance);

                __instance.Storage.TryGetValue("$rareCard1", out string rareCard1);
                if (rareCard1 == null) return;

                __instance.Storage.TryGetValue("$isUpgraded", out bool isUpgraded);
                CardObj card = new CardObj()
                {
                    Id = rareCard1,
                    IsUpgraded = isUpgraded
                };
                RunDataController.AddData("Card", card);
                RunDataController.AddData("Cards", rareCards);

                RunDataController.Listener = nameof(SumirekoGathering);
            }

            [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.HasMoney)), HarmonyPostfix]
            public static void HasMoneyPatch(bool __result, GameRunController ____gameRun)
            {
                Station station = ____gameRun.CurrentStation;
                Adventure adv = null;
                if (station is AdventureStation) adv = (station as AdventureStation).Adventure;
                else if (station is BattleAdvTestStation) adv = (station as BattleAdvTestStation).Adventure;
                if (adv == null) return;
                RunDataController.AddData("HasMoney", __result);
                if (__result)
                {
                    if (RunDataController.CurrentStation.Data.TryGetValue("Cards", out object cards)) return;
                    RunDataController.AddData("Cards", rareCards);
                }
            }

            private static List<string> GetRareCards(SumirekoGathering sumirekoGathering)
            {
                sumirekoGathering.Storage.TryGetValue("$rareTrade1", out string rareTrade1);
                sumirekoGathering.Storage.TryGetValue("$rareTrade2", out string rareTrade2);
                sumirekoGathering.Storage.TryGetValue("$rareTrade3", out string rareTrade3);
                List<string> rareCards = new List<string> { rareTrade1, rareTrade2, rareTrade3 };
                return rareCards;
            }
        }

        [HarmonyPatch(typeof(ShinmyoumaruForge))]
        public static class ShinmyoumaruForgePatch
        {
            [HarmonyPatch(nameof(ShinmyoumaruForge.InitVariables))]
            public static void Postfix(ShinmyoumaruForge __instance)
            {
                __instance.Storage.TryGetValue("$hasUpgradableBasics", out bool hasUpgradableBasics);
                RunDataController.AddData("HasUpgradableBasics", hasUpgradableBasics);
                __instance.Storage.TryGetValue("$hasNonBasics", out bool hasNonBasics);
                RunDataController.AddData("HasNonBasics", hasNonBasics);
                __instance.Storage.TryGetValue("$loseMax", out float loseMax);
                RunDataController.AddData("LoseMax", (int)loseMax);
            }
        }
    }
}
