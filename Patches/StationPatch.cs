using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.GapOptions;
using LBoL.Core.Stations;
using LBoL.Presentation.UI.Panels;
using RunLogger.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Station))]
    class StationPatch
    {
        public const string Listener = "AddReward";

        [HarmonyPatch(nameof(Station.AddReward)), HarmonyPrefix]
        static void AddRewardPatch(StationReward reward)
        {
            RunDataController.Listener = Listener;
            RewardsUtil.AddReward(reward);
            RunDataController.Listener = null;
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
            RunDataController.Listener = "GetRareCard";
        }

        [HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        static void ShowMiniSelectPatch(SelectCardPayload payload)
        {
            switch (RunDataController.Listener)
            {
                case "GetRareCard":
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
        private static bool appended;

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterMapNode)), HarmonyPostfix, HarmonyPriority(1)]
        static void EnterMapNodePatch(GameRunController __instance)
        {
            Station CurrentStation = __instance.CurrentStation;
            if (!(CurrentStation is ShopStation)) return;

            appended = false;

            ShopStation station = CurrentStation as ShopStation;
            List<ShopItem<Card>> cardsList = station.ShopCards;
            List<CardWithPrice> cards = cardsList.Select(item => {
                int price = item.Price;
                return RunDataController.GetCardWithPrice(item.Content, price);
            }).ToList();
            cards[station.DiscountCardNo].IsDiscounted = true;

            List<ShopItem<Exhibit>> exhibitsList = station.ShopExhibits;
            List<string> exhibits = exhibitsList.Select(item => item.Content.Id).ToList();
            Dictionary<string, int> prices = new Dictionary<string, int>()
            {
                { "Remove", station.RemoveDeckCardPrice },
                { "Upgrade", station.UpgradeDeckCardPrice }
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
            private const string Listener = "BuyCard";

            [HarmonyPatch(nameof(ShopStation.BuyCard))]
            static void Prefix(ShopItem<Card> cardItem, ShopStation __instance)
            {
                //RunDataController.AddDataItem("Cards", RunDataController.GetCard(cardItem.Content));
                RunDataController.Listener = Listener;
            }

            [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Card), typeof(bool) }), HarmonyPostfix]
            static void GetPricePatch(Card card, int __result)
            {
                if (RunDataController.Listener != Listener) return;
                RunDataController.CurrentStation.Rewards.TryGetValue("Cards", out object value);
                List<List<CardWithPrice>> cards = value as List<List<CardWithPrice>>;
                if (appended == false)
                {
                    cards.Add(new List<CardWithPrice>());
                    appended = true;
                }
                cards[^1].Add(RunDataController.GetCardWithPrice(card, __result));
                RunDataController.Listener = null;
            }
        }

        [HarmonyDebug]
        [HarmonyPatch(typeof(ShopStation))]
        class BuyExhibitRunnerPatch
        {
            private const string Listener = "BuyExhibit";

            [HarmonyPatch(nameof(ShopStation.BuyExhibitRunner))]
            static void Prefix(ShopItem<Exhibit> exhibitItem, ShopStation __instance)
            {
                //RunDataController.AddDataItem("Exhibits", exhibitItem.Content.Id);
                RunDataController.Listener = Listener;
            }

            [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Exhibit) }), HarmonyPostfix]
            static void GetPricePatch(Exhibit exhibit, int __result)
            {
                if (RunDataController.Listener != Listener) return;
                RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out object exhibits);
                RunDataController.AddPrice(exhibit.Id, __result);
                (exhibits as List<string>).Add(exhibit.Id);
                RunDataController.Listener = null;
            }
        }
    }
}