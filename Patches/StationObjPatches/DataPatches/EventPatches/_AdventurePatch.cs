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