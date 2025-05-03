using HarmonyLib;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Dialogs;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.EntityLib.Adventures.Shared12;
using LBoL.EntityLib.Adventures.Shared23;
using LBoL.EntityLib.Adventures.Stage1;
using LBoL.EntityLib.Adventures.Stage2;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.Presentation.UI.Panels;
using RunLogger.Legacy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch(typeof(Adventure))]
    public static class AdventurePatch
    {
        //[HarmonyPatch("InitVariables")]
        //public static void Postfix(Adventure __instance)
        //{
        //    string Id = __instance.Id;
        //    string[] manaEvents = new[] { "JunkoColorless", "PatchouliPhilosophy" };
        //    if (manaEvents.Contains(Id))
        //    {
        //        GameRunController gameRun = __instance.GameRun;
        //        string[] exhibits = gameRun.ExhibitRecord.ToArray();
        //        string baseMana = RunDataController.GetBaseMana(gameRun.BaseMana.ToString(), exhibits);
        //        RunDataController.AddData("BaseMana", baseMana);
        //    }
        //}

        //[HarmonyPatch(typeof(Debut))]
        //public static class DebutPatch
        //{
        //    private static int uncommonCardsIndex;
        //    public static bool uncommonCardListener;

        //    [HarmonyPatch(nameof(Debut.RollBonus))]
        //    public static void Postfix(Exhibit ____exhibit, int[] ____bonusNos, Debut __instance)
        //    {
        //        if (!RunDataController.RunData.Settings.HasClearBonus) return;
        //        Exhibit _exhibit = ____exhibit;
        //        int[] _bonusNos = ____bonusNos;
        //        RunDataController.AddData("Options", _bonusNos);

        //        int i = Array.FindIndex(_bonusNos, _bonusNo => _bonusNo == 0);
        //        if (i != -1) uncommonCardsIndex = i + 2;

        //        if (!RunDataController.ShowRandom) return;

        //        DialogStorage storage = __instance.Storage;
        //        RunDataController.AddData("Shining", _exhibit.Id);
        //        foreach (int _bonusNo in _bonusNos)
        //        {
        //            switch (_bonusNo)
        //            {
        //                case 0:
        //                    List<string> uncommonCards = RunDataController.GetList<string>(storage, new[] { 1, 2, 3 }, "$uncommonCard");
        //                    RunDataController.AddData("UncommonCards", uncommonCards);
        //                    break;
        //                case 1:
        //                    storage.TryGetValue("$rareCard", out string rareCard);
        //                    RunDataController.AddData("RareCard", rareCard);
        //                    break;
        //                case 2:
        //                    storage.TryGetValue("$rareExhibit", out string rareExhibit);
        //                    RunDataController.AddData("RareExhibit", rareExhibit);
        //                    break;
        //                case 5:
        //                    storage.TryGetValue("$transformCard", out string transformCard);
        //                    RunDataController.AddData("TransformCard", transformCard);
        //                    break;
        //            }
        //        }
        //    }

        //    [HarmonyPatch(typeof(DialogRunner), nameof(DialogRunner.SelectOption))]
        //    public static class SelectOptionPatch
        //    {
        //        public static void Prefix(int id)
        //        {
        //            if (uncommonCardsIndex != 0 && id == uncommonCardsIndex) uncommonCardListener = true;
        //        }

        //        public static void Postfix()
        //        {
        //            uncommonCardsIndex = 0;
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(Supply))]
        //public static class SupplyPatch
        //{
        //    [HarmonyPatch(nameof(Supply.InitVariables))]
        //    public static void Postfix(Supply __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        storage.TryGetValue("$exhibitA", out string exhibitA);
        //        storage.TryGetValue("$exhibitB", out string exhibitB);
        //        RunDataController.AddData("Exhibits", new[] { exhibitA, exhibitB });

        //        storage.TryGetValue("$bothFlag", out bool bothFlag);
        //        RunDataController.AddData("Both", bothFlag);
        //    }
        //}

        //[HarmonyPatch(typeof(RinnosukeTrade))]
        //public static class RinnosukeTradePatch
        //{
        //    [HarmonyPatch(nameof(RinnosukeTrade.InitVariables))]
        //    public static void Postfix(Supply __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        storage.TryGetValue("$canSell1", out bool canSell1);
        //        storage.TryGetValue("$canSell2", out bool canSell2);

        //        if (canSell1)
        //        {
        //            Dictionary<string, int> prices = new Dictionary<string, int>();

        //            storage.TryGetValue("$exhibit1", out string exhibit1);
        //            storage.TryGetValue("$exhibit1Price", out float exhibit1Price);
        //            prices.Add(exhibit1, (int)exhibit1Price);

        //            if (canSell2)
        //            {
        //                storage.TryGetValue("$exhibit2", out string exhibit2);
        //                storage.TryGetValue("$exhibit2Price", out float exhibit2Price);
        //                prices.Add(exhibit2, (int)exhibit2Price);
        //            }

        //            RunDataController.AddData("Prices", prices);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(DoremyPortal))]
        //public static class DoremyPortalPatch
        //{
        //    [HarmonyPatch(nameof(DoremyPortal.InitVariables))]
        //    public static void Postfix(DoremyPortal __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$randomExhibit", out string randomExhibit);
        //            RunDataController.AddData("Exhibit", randomExhibit);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(SelectBaseManaInteraction), nameof(SelectBaseManaInteraction.SelectedMana), MethodType.Setter)]
        //public static class SelectBaseManaInteractionPatch
        //{
        //    static void Prefix(ManaGroup value)
        //    {
        //        ManaGroup mana = value;
        //        string color = mana.MaxColor.ToShortName().ToString();
        //        RunDataController.AddData("Color", color);
        //    }

        //    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //    {
        //        return instructions;
        //    }
        //}

        //[HarmonyPatch(typeof(MiyoiBartender))]
        //public static class MiyoiBartenderPatch
        //{
        //    private static string exhibit;

        //    [HarmonyPatch(nameof(MiyoiBartender.InitVariables))]
        //    public static void Prefix(MiyoiBartender __instance)
        //    {
        //        UniqueRandomPool<string> uniqueRandomPool = __instance.Stage.EnemyPoolAct3;
        //        List<string> ids = uniqueRandomPool.Select((e) => e.Elem).ToList();
        //        RunDataController.AddData("Ids", ids);
        //    }

        //    [HarmonyPatch(nameof(MiyoiBartender.InitVariables))]
        //    public static void Postfix(MiyoiBartender __instance)
        //    {
        //        __instance.Storage.TryGetValue("$randomExhibit", out string randomExhibit);
        //        if (RunDataController.ShowRandom)
        //        {
        //            RunDataController.AddData("Exhibit", randomExhibit);
        //            exhibit = null;
        //        }
        //        else
        //        {
        //            exhibit = randomExhibit;
        //        }
        //    }

        //    public static void HandleBattle()
        //    {
        //        if (exhibit == null) return;
        //        RunDataController.AddData("Exhibit", exhibit);
        //    }
        //}

        //[HarmonyPatch(typeof(SumirekoGathering))]
        //public static class SumirekoGatheringPatch
        //{
        //    private static List<string> rareCards;

        //    [HarmonyPatch(nameof(SumirekoGathering.InitVariables))]
        //    public static void Postfix(SumirekoGathering __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        rareCards = RunDataController.GetList<string>(storage, new[] { 1, 2, 3 }, "$rareTrade");

        //        storage.TryGetValue("$rareCard1", out string rareCard1);
        //        if (rareCard1 == null) return;

        //        storage.TryGetValue("$isUpgraded", out bool isUpgraded);
        //        CardObj card = new CardObj()
        //        {
        //            Id = rareCard1,
        //            IsUpgraded = isUpgraded
        //        };
        //        RunDataController.AddData("Card", card);
        //        RunDataController.AddData("Cards", rareCards);

        //        InteractionViewerPatch.Listener = nameof(SumirekoGathering);
        //    }

        //    [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.HasMoney)), HarmonyPostfix]
        //    public static void HasMoneyPatch(bool __result, GameRunController ____gameRun)
        //    {
        //        Station station = ____gameRun.CurrentStation;
        //        string Id = RunDataController.GetAdventureId(station);
        //        if (Id == null) return;
        //        else if (Id != nameof(SumirekoGathering)) return;

        //        RunDataController.AddData("HasMoney", __result);
        //        if (__result)
        //        {
        //            if (RunDataController.CurrentStation.Data.TryGetValue("Cards", out object cards)) return;
        //            RunDataController.AddData("Cards", rareCards);

        //            InteractionViewerPatch.Listener = nameof(SumirekoGathering);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(ShinmyoumaruForge))]
        //public static class ShinmyoumaruForgePatch
        //{
        //    [HarmonyPatch(nameof(ShinmyoumaruForge.InitVariables))]
        //    public static void Postfix(ShinmyoumaruForge __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        storage.TryGetValue("$hasUpgradableBasics", out bool hasUpgradableBasics);
        //        RunDataController.AddData("HasUpgradableBasics", hasUpgradableBasics);
        //        storage.TryGetValue("$hasNonBasics", out bool hasNonBasics);
        //        RunDataController.AddData("HasNonBasics", hasNonBasics);
        //        storage.TryGetValue("$loseMax", out float loseMax);
        //        RunDataController.AddData("LoseMax", (int)loseMax);
        //    }
        //}

        //[HarmonyPatch(typeof(WatatsukiPurify))]
        //public static class WatatsukiPurifyPatch
        //{
        //    [HarmonyPatch(nameof(WatatsukiPurify.InitVariables))]
        //    public static void Postfix(WatatsukiPurify __instance)
        //    {
        //        __instance.Storage.TryGetValue("$loseMax", out float loseMax);
        //        RunDataController.AddData("LoseMax", (int)loseMax);
        //    }
        //}

        //[HarmonyPatch(typeof(HinaCollect))]
        //public static class HinaCollectPatch
        //{
        //    [HarmonyPatch(nameof(HinaCollect.InitVariables))]
        //    public static void Postfix(HinaCollect __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$card", out string card);
        //            RunDataController.AddData("Card", card);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(KosuzuBookstore))]
        //public static class KosuzuBookstorePatch
        //{
        //    [HarmonyPatch(nameof(KosuzuBookstore.InitVariables))]
        //    public static void Postfix(KosuzuBookstore __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        storage.TryGetValue("$thirdBook", out bool thirdBook);
        //        storage.TryGetValue("$book0", out string book0);
        //        storage.TryGetValue("$book1", out string book1);
        //        List<string> exhibits = new List<string>() { book0, book1 };
        //        if (thirdBook)
        //        {
        //            storage.TryGetValue("$book2", out string book2);
        //            exhibits.Add(book2);
        //        }
        //        RunDataController.AddData("Exhibits", exhibits);

        //        storage.TryGetValue("$returnBookCount", out float returnBookCount);
        //        int k = (int)returnBookCount;
        //        if (k > 0)
        //        {
        //            List<string> returns = RunDataController.GetList<string>(storage, Enumerable.Range(0, k), "$returnBook");
        //            RunDataController.AddData("Returns", returns);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(NarumiOfferCard))]
        //public static class NarumiOfferCardPatch
        //{
        //    public static bool isNarumi;

        //    [HarmonyPatch(nameof(NarumiOfferCard.OfferDeckCard))]
        //    public static void Prefix()
        //    {
        //        isNarumi = true;
        //    }

        //    [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.RemoveDeckCards)), HarmonyPostfix]
        //    public static void RemoveDeckCardsPatch(IEnumerable<Card> cards)
        //    {
        //        if (!isNarumi) return;

        //        string type;
        //        Card card = cards.First();
        //        if (card.CardType == CardType.Misfortune) type = CardType.Misfortune.ToString();
        //        else type = card.Config.Rarity.ToString();
        //        RunDataController.AddData("Type", type);
        //        isNarumi = false;
        //    }
        //}

        //[HarmonyPatch]
        //public static class NazrinDetectPatch
        //{
        //    [HarmonyPatch(typeof(NazrinDetectPanel), nameof(NazrinDetectPanel.Roll)), HarmonyPostfix]
        //    public static void RollPatch(int resultIndex)
        //    {
        //        RunDataController.AddData("Result", resultIndex);
        //    }
        //}

        //[HarmonyPatch]
        //public static class HecatiaTshirtPatch
        //{
        //    [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.GainExhibitRunner)), HarmonyPostfix, HarmonyPriority(1)]
        //    static void GainExhibitRunnerPatch(Exhibit exhibit)
        //    {
        //        if (!(exhibit is IdolTshirt)) return;
        //        int counter = exhibit.Counter;
        //        if (counter > 2) RunDataController.AddExhibitChange(exhibit, ChangeType.Upgrade, counter);
        //    }
        //}

        //[HarmonyPatch(typeof(KeineSales))]
        //public static class KeineSalesPatch
        //{
        //    [HarmonyPatch(nameof(KeineSales.InitVariables))]
        //    public static void Postfix(KeineSales __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        storage.TryGetValue("$stageNo", out float stageNo);
        //        List<int> keys = new List<int>() { 1 };
        //        if (stageNo > 1) keys.Add(2);
        //        List<int> questions = RunDataController.GetList<float>(storage, keys, "$question", "No").Select(question => (int)question).ToList();
        //        RunDataController.AddData("Questions", questions);
        //    }
        //}

        //[HarmonyPatch(typeof(MikeInvest))]
        //public static class MikeInvestPatch
        //{
        //    [HarmonyPatch(nameof(MikeInvest.InitVariables))]
        //    public static void Postfix(MikeInvest __instance)
        //    {
        //        DialogStorage storage = __instance.Storage;
        //        storage.TryGetValue("$longMoney", out float longMoney);
        //        RunDataController.AddData("Money", (int)longMoney);
        //        if (RunDataController.ShowRandom)
        //        {
        //            storage.TryGetValue("$cardReward", out string cardReward);
        //            RunDataController.AddData("Card", cardReward);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(YoumuDelivery))]
        //public static class YoumuDeliveryPatch
        //{
        //    [HarmonyPatch(nameof(YoumuDelivery.InitVariables))]
        //    public static void Postfix(YoumuDelivery __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$transformCard", out string transformCard);
        //            RunDataController.AddData("Card", transformCard);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(RemiliaMeet))]
        //public static class RemiliaMeetPatch
        //{
        //    [HarmonyPatch(nameof(RemiliaMeet.InitVariables))]
        //    public static void Postfix(RemiliaMeet __instance)
        //    {
        //        __instance.Storage.TryGetValue("$hasExhibit", out bool hasExhibit);
        //        RunDataController.AddData("HasExhibit", hasExhibit);
        //    }
        //}

        //[HarmonyPatch(typeof(RingoEmp))]
        //public static class RingoEmpPatch
        //{
        //    [HarmonyPatch(nameof(RingoEmp.InitVariables))]
        //    public static void Postfix(RingoEmp __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            DialogStorage storage = __instance.Storage;
        //            List<string> cards = RunDataController.GetList<string>(storage, new[] { 1, 2, 3 }, "$tool");
        //            RunDataController.AddData("Cards", cards);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(YachieOppression))]
        //public static class YachieOppressionPatch
        //{
        //    private static string exhibit;

        //    [HarmonyPatch(nameof(YachieOppression.InitVariables))]
        //    public static void Postfix(YachieOppression __instance)
        //    {
        //        __instance.Storage.TryGetValue("$enemyExhibit", out string enemyExhibit);
        //        if (RunDataController.ShowRandom) RunDataController.AddData("Exhibit", enemyExhibit);
        //        else exhibit = enemyExhibit;
        //    }

        //    public static void HandleBattle()
        //    {
        //        if (exhibit == null) return;
        //        RunDataController.AddData("Exhibit", exhibit);
        //        exhibit = null;
        //    }
        //}

        //[HarmonyPatch(typeof(AssistKagerou))]
        //public static class AssistKagerouPatch
        //{
        //    [HarmonyPatch(nameof(AssistKagerou.InitVariables))]
        //    public static void Postfix(AssistKagerou __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$exhibitReward", out string exhibitReward);
        //            RunDataController.AddData("Exhibit", exhibitReward);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(EternityAscension))]
        //public static class EternityAscensionPatch
        //{
        //    [HarmonyPatch(nameof(EternityAscension.InitVariables))]
        //    public static void Postfix(EternityAscension __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$transformCard", out string transformCard);
        //            RunDataController.AddData("Card", transformCard);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(KaguyaVersusMokou))]
        //public static class KaguyaVersusMokouPatch
        //{
        //    [HarmonyPatch(nameof(KaguyaVersusMokou.InitVariables))]
        //    public static void Postfix(KaguyaVersusMokou __instance)
        //    {
        //        __instance.Storage.TryGetValue("$hpLose", out float hpLose);
        //        __instance.Storage.TryGetValue("$hpLoseLow", out float hpLoseLow);
        //        RunDataController.AddData("Hps", new[] { (int)hpLose, (int)hpLoseLow });
        //    }
        //}

        //[HarmonyPatch(typeof(ParseeJealousy))]
        //public static class ParseeJealousyPatch
        //{
        //    [HarmonyPatch(nameof(ParseeJealousy.InitVariables))]
        //    public static void Postfix(ParseeJealousy __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$exhibitPassBy", out string exhibitPassBy);
        //            RunDataController.AddData("Exhibit", exhibitPassBy);
        //        }
        //    }

        //    [HarmonyPatch(nameof(ParseeJealousy.GetExhibit)), HarmonyPostfix]
        //    public static void GetExhibitPatch(ParseeJealousy __instance)
        //    {
        //        __instance.Storage.TryGetValue("$exhibit", out string exhibit);
        //        __instance.Storage.TryGetValue("$exhibit2", out string exhibit2);
        //        RunDataController.AddData("Exhibits", new[] { exhibit, exhibit2 });
        //    }
        //}
    }
}