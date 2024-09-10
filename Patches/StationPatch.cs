using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using RunLogger.Debug;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Station))]
    class StationPatch
    {
        public const string Listener = nameof(Station.AddReward);

        [HarmonyPatch(nameof(Station.AddReward)), HarmonyPrefix]
        static void AddRewardPatch(StationReward reward)
        {
            if (RunDataController.Listener != null) return;
            RunDataController.Listener = Listener;
            RewardsUtil.AddReward(reward);
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(Station))]
        class AddRewarsdPatchPatch
        {
            [HarmonyPatch(nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) })]
            static void Prefix(IEnumerable<StationReward> rewards)
            {
                RunDataController.Listener = Listener;
                RewardsUtil.AddRewards(rewards.ToList());
            }

            [HarmonyPatch(nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) })]
            static void Postfix()
            {
                RunDataController.Listener = null;
                StagePatch.waitForSave = true;
            }
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(BossStation))]
    class BossStationPatch
    {
        [HarmonyPatch(nameof(BossStation.GenerateBossRewards)), HarmonyPostfix]
        static void GenerateBossRewardsPatch(BossStation __instance)
        {
            Exhibit[] BossRewards = __instance.BossRewards;
            List<string> Exhibits = BossRewards.Select(exhibit => exhibit.Id).ToList();
            Dictionary<string, object> Rewards = new Dictionary<string, object>
            {
                { "Exhibits", Exhibits }
            };
            RunDataController.CurrentStation.Rewards = Rewards;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(GapStation))]
    class GapStationPatch
    {
        private const string Listener = nameof(GapOptionsPanel.InternalGerRareCard);

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        static void OptionClickedPatch(GapOption option, GapOptionsPanel __instance)
        {
            string Choice = option.Type.ToString();
            List<string> Options = __instance.GapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
            RunDataController.AddData("Choice", Choice);
            RunDataController.AddData("Options", Options);
        }

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.InternalGerRareCard)), HarmonyPrefix]
        static void InternalGerRareCardPatch()
        {
            RunDataController.Listener = Listener;
        }

        [HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        static void ShowMiniSelectPatch(SelectCardPayload payload)
        {
            switch (RunDataController.Listener)
            {
                case Listener:
                    List<CardObj> cards = RunDataController.GetCards(payload.Cards);
                    RunDataController.AddData("ShanliangDengpao", cards);
                    break;
            }
            RunDataController.Listener = null;
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(SelectStation))]
    class SelectStationPatch
    {
        [HarmonyPatch(nameof(SelectStation.GenerateRecord)), HarmonyPostfix]
        static void GenerateRecordPatch(SelectStation __instance)
        {
            List<string> Opponents = __instance.Opponents.Select(opponent =>  opponent.Id).ToList();
            RunDataController.AddData("Opponents", Opponents);
        }
    }

    [HarmonyDebug]
    [HarmonyPatch(typeof(ShopStation))]
    class ShopStationPatch
    {
        private static string Listener;
        private static int index = -1;

        private static void Reset()
        {
            Debugger.Write("Reset");
            Listener = null;
            index = -1;
        }

        [HarmonyPatch(typeof(ShopStation), nameof(ShopStation.OnEnter))]
        class OnEnterPatch
        {
            static void Prefix(ShopStation __instance)
            {
                Debugger.Write("Pre OnEnter");
                Reset();
            }

            static void Postfix(ShopStation __instance)
            {
                Debugger.Write("Post OnEnter");

                List<ShopItem<Card>> cardsList = __instance.ShopCards;
                List<CardWithPrice> cards = cardsList.Select(item =>
                {
                    int price = item.Price;
                    return RunDataController.GetCardWithPrice(item.Content, price);
                }).ToList();
                cards[__instance.DiscountCardNo].IsDiscounted = true;

                List<ShopItem<Exhibit>> exhibitsList = __instance.ShopExhibits;
                List<string> exhibits = exhibitsList.Select(item => item.Content.Id).ToList();
                Dictionary<string, int> prices = new Dictionary<string, int>()
                {
                    { "Remove", __instance.RemoveDeckCardPrice },
                    { "Upgrade", __instance.UpgradeDeckCardPrice }
                };
                foreach (ShopItem<Exhibit> item in exhibitsList) prices[item.Content.Id] = item.Price;

                Dictionary<string, object> rewards = new Dictionary<string, object>()
                {
                    { "Cards", new List<List<CardWithPrice>>() { cards } },
                    { "Exhibits", exhibits }
                };

                RunDataController.CurrentStation.Rewards = rewards;
                RunDataController.AddData("Prices", prices);
            }
        }

        [HarmonyPatch(nameof(ShopStation.RemoveDeckCard)), HarmonyPrefix]
        static void RemoveDeckCardPatch(Card card)
        {
            RunDataController.AddData("Choice", 0);
            RunDataController.AddData("Remove", RunDataController.GetCard(card));
        }

        [HarmonyPatch(nameof(ShopStation.UpgradeDeckCard)), HarmonyPrefix]
        static void UpgradeDeckCardPatch(Card card)
        {
            RunDataController.AddData("Choice", 1);
            RunDataController.AddData("Upgrade", RunDataController.GetCard(card));
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(ShopStation))]
        class BuyCardPatch
        {
            private const string BuyCard = nameof(ShopStation.BuyCard);

            [HarmonyPatch(nameof(ShopStation.BuyCard))]
            static void Prefix(ShopItem<Card> cardItem, ShopStation __instance)
            {
                Listener = BuyCard;
            }

            [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Card), typeof(bool) }), HarmonyPostfix]
            static void GetPricePatch(Card card, int __result)
            {
                if (Listener != BuyCard) return;
                RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out object value);
                List<List<CardWithPrice>> cards = value as List<List<CardWithPrice>>;
                if (index == -1)
                {
                    index = cards.Count;
                    cards.Add(new List<CardWithPrice>());
                }
                cards[index].Add(RunDataController.GetCardWithPrice(card, __result));
                Reset();
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(ShopStation))]
        class BuyExhibitRunnerPatch
        {
            private const string BuyExhibit = nameof(ShopStation.BuyExhibitRunner);

            [HarmonyPatch(nameof(ShopStation.BuyExhibitRunner))]
            static void Prefix(ShopItem<Exhibit> exhibitItem, ShopStation __instance)
            {
                Debugger.Write($"BuyExhibit");
                Debugger.Write($"Item is null: {exhibitItem==null}");
                Debugger.Write($"Content is null: {exhibitItem.Content == null}");
                Debugger.Write($"Id: {exhibitItem.Content.Id}");
                Listener = BuyExhibit;
            }

            [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Exhibit) }), HarmonyPostfix]
            static void GetPricePatch(Exhibit exhibit, int __result)
            {
                Debugger.Write($"Listener: {Listener}");
                if (Listener != BuyExhibit) return;
                Debugger.Write("A");
                RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out object exhibits);
                Debugger.Write("B");
                RunDataController.AddPrice(exhibit.Id, __result);
                Debugger.Write("C");
                (exhibits as List<string>).Add(exhibit.Id);
                Debugger.Write("D");
                Reset();
                Debugger.Write("E");
            }
        }
    }
}