using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using RunLogger.Legacy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Legacy.Patches
{
    [HarmonyPatch(typeof(BossStation))]
    class _BossStationPatch
    {
        //[HarmonyPatch(nameof(BossStation.GenerateBossRewards)), HarmonyPostfix]
        //static void GenerateBossRewardsPatch(BossStation __instance)
        //{
        //    Exhibit[] BossRewards = __instance.BossRewards;
        //    List<string> Exhibits = BossRewards.Select(exhibit => exhibit.Id).ToList();
        //    Dictionary<string, object> Rewards = new Dictionary<string, object>
        //    {
        //        { "Exhibits", Exhibits }
        //    };
        //    RunDataController.CurrentStation.Rewards = Rewards;
        //}
    }

    [HarmonyPatch(typeof(GapStation))]
    public static class _GapStationPatch
    {
        //private const string Listener = nameof(GapOptionsPanel.InternalGerRareCard);
        //public static Action UpgradeMoping;

        //[HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OnShowing)), HarmonyPostfix]
        //static void OnShowingPatch(GapStation gapStation)
        //{
        //    List<string> Options = gapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
        //    RunDataController.AddData("Options", Options);
        //    if (UpgradeMoping != null)
        //    {
        //        UpgradeMoping();
        //        UpgradeMoping = null;
        //    }
        //}

        //[HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        //static void OptionClickedPatch(GapOption option)
        //{
        //    string Choice = option.Type.ToString();
        //    RunDataController.CurrentStation.Data["Choice"] = Choice;
        //}

        //[HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.InternalGerRareCard)), HarmonyPrefix]
        //static void InternalGerRareCardPatch()
        //{
        //    StationPatch.RewardListener = Listener;
        //}

        //[HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        //static void ShowMiniSelectPatch(SelectCardPayload payload)
        //{
        //    string RewardListener = StationPatch.RewardListener;
        //    BepinexPlugin.log.LogDebug($"RewardListener in {System.Reflection.MethodBase.GetCurrentMethod().Name}: {RewardListener}");
        //    switch (RewardListener)
        //    {
        //        case Listener:
        //            List<CardObj> cards = RunDataController.GetCards(payload.Cards);
        //            RunDataController.AddData("ShanliangDengpao", cards);
        //            break;
        //    }
        //    StationPatch.RewardListener = null;
        //}
    }

    [HarmonyPatch(typeof(SelectStation))]
    class _SelectStationPatch
    {
        //[HarmonyPatch(nameof(SelectStation.GenerateRecord)), HarmonyPostfix]
        //static void GenerateRecordPatch(SelectStation __instance)
        //{
        //    List<string> Opponents = __instance.Opponents.Select(opponent =>  opponent.Id).ToList();
        //    RunDataController.AddData("Opponents", Opponents);
        //}
    }

    [HarmonyPatch(typeof(ShopStation))]
    class _ShopStationPatch
    {
        //private static string Listener;
        //private static int index;

        //[HarmonyPatch(nameof(ShopStation.OnEnter)), HarmonyPrefix]
        //static void OnEnterPatch()
        //{
        //    Listener = null;
        //    index = -1;
        //}

        //[HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterMapNode)), HarmonyPostfix, HarmonyPriority(1)]
        //static void EnterMapNodePatch(GameRunController __instance)
        //{
        //    Station CurrentStation = __instance.CurrentStation;
        //    if (!(CurrentStation is ShopStation station)) return;

        //    List<ShopItem<Card>> cardsList = station.ShopCards;
        //    List<CardWithPrice> cards = cardsList.Select(item =>
        //    {
        //        int price = item.Price;
        //        return RunDataController.GetCardWithPrice(item.Content, price);
        //    }).ToList();
        //    cards[station.DiscountCardNo].IsDiscounted = true;

        //    List<ShopItem<Exhibit>> exhibitsList = station.ShopExhibits;
        //    List<string> exhibits = exhibitsList.Select(item => item.Content.Id).ToList();
        //    Dictionary<string, int> prices = new Dictionary<string, int>()
        //    {
        //        { "Remove", station.RemoveDeckCardPrice },
        //        { "Upgrade", station.UpgradeDeckCardPrice }
        //    };
        //    foreach (ShopItem<Exhibit> item in exhibitsList) prices[item.Content.Id] = item.Price;

        //    Dictionary<string, object> rewards = new Dictionary<string, object>()
        //    {
        //        { "Cards", new List<List<CardWithPrice>>() { cards } },
        //        { "Exhibits", exhibits }
        //    };

        //    RunDataController.CurrentStation.Rewards = rewards;
        //    RunDataController.AddData("Prices", prices);
        //}

        //[HarmonyPatch(nameof(ShopStation.RemoveDeckCard)), HarmonyPrefix]
        //static void RemoveDeckCardPatch(Card card)
        //{
        //    RunDataController.AddData("Choice", 0);
        //    RunDataController.AddData("Remove", RunDataController.GetCard(card));
        //}

        //[HarmonyPatch(nameof(ShopStation.UpgradeDeckCard)), HarmonyPrefix]
        //static void UpgradeDeckCardPatch(Card card)
        //{
        //    RunDataController.AddData("Choice", 1);
        //    RunDataController.AddData("Upgrade", RunDataController.GetCard(card));
        //}

        //[HarmonyPatch(typeof(ShopStation))]
        //class BuyCardPatch
        //{
        //    private const string BuyCard = nameof(ShopStation.BuyCard);

        //    [HarmonyPatch(nameof(ShopStation.BuyCard))]
        //    static void Prefix()
        //    {
        //        Listener = BuyCard;
        //    }

        //    [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Card), typeof(bool) }), HarmonyPostfix]
        //    static void GetPricePatch(Card card, int __result, ShopStation __instance)
        //    {
        //        if (Listener != BuyCard) return;
        //        RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out object value);
        //        List<List<CardWithPrice>> cards = value as List<List<CardWithPrice>>;
        //        if (index == -1)
        //        {
        //            index = cards.Count;
        //            cards.Add(new List<CardWithPrice>());
        //        }
        //        int price = (int)(__instance.GameRun.FinalShopPriceMultiplier * __result);
        //        cards[index].Add(RunDataController.GetCardWithPrice(card, price));
        //        Listener = null;
        //    }
        //}

        //[HarmonyPatch(typeof(ShopStation))]
        //class BuyExhibitRunnerPatch
        //{
        //    private const string BuyExhibit = nameof(ShopStation.BuyExhibitRunner);

        //    [HarmonyPatch(nameof(ShopStation.BuyExhibitRunner))]
        //    static void Prefix()
        //    {
        //        Listener = BuyExhibit;
        //    }

        //    [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Exhibit) }), HarmonyPostfix]
        //    static void GetPricePatch(Exhibit exhibit, int __result, ShopStation __instance)
        //    {
        //        if (Listener != BuyExhibit) return;
        //        RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out object exhibits);
        //        int price = (int)(__instance.GameRun.FinalShopPriceMultiplier * __result);
        //        RunDataController.AddPrice(exhibit.Id, price);
        //        (exhibits as List<string>).Add(exhibit.Id);
        //        Listener = null;
        //    }
        //}
    }
}