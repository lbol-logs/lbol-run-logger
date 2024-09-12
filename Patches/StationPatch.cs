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
    [HarmonyPatch(typeof(Station))]
    public static class StationPatch
    {
        public static string RewardListener;
        public const string Listener = nameof(Station.AddReward);

        [HarmonyPatch(nameof(Station.AddReward)), HarmonyPrefix]
        static void AddRewardPatch(StationReward reward)
        {
            if (StationPatch.RewardListener != null) return;
            StationPatch.RewardListener = Listener;
            RewardsUtil.AddReward(reward);
        }

        [HarmonyPatch(typeof(Station))]
        class AddRewarsdPatchPatch
        {
            [HarmonyPatch(nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) })]
            static void Prefix(IEnumerable<StationReward> rewards)
            {
                StationPatch.RewardListener = Listener;
                RewardsUtil.AddRewards(rewards.ToList());
            }

            [HarmonyPatch(nameof(Station.AddRewards), new Type[] { typeof(IEnumerable<StationReward>) })]
            static void Postfix()
            {
                StationPatch.RewardListener = null;
                StagePatch.waitForSave = true;
            }
        }
    }

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

    [HarmonyPatch(typeof(GapStation))]
    class GapStationPatch
    {
        private const string Listener = nameof(GapOptionsPanel.InternalGerRareCard);

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OnShowing)), HarmonyPostfix]
        static void OnShowingPatch(GapStation gapStation)
        {
            List<string> Options = gapStation.GapOptions.Select(gapOption => gapOption.Type.ToString()).ToList();
            RunDataController.AddData("Options", Options);
        }

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.OptionClicked)), HarmonyPostfix]
        static void OptionClickedPatch(GapOption option, GapOptionsPanel __instance)
        {
            string Choice = option.Type.ToString();
            RunDataController.CurrentStation.Data["Choice"] = Choice;
        }

        [HarmonyPatch(typeof(GapOptionsPanel), nameof(GapOptionsPanel.InternalGerRareCard)), HarmonyPrefix]
        static void InternalGerRareCardPatch()
        {
            StationPatch.RewardListener = Listener;
        }

        [HarmonyPatch(typeof(SelectCardPanel), nameof(SelectCardPanel.ShowMiniSelect)), HarmonyPrefix]
        static void ShowMiniSelectPatch(SelectCardPayload payload)
        {
            switch (StationPatch.RewardListener)
            {
                case Listener:
                    List<CardObj> cards = RunDataController.GetCards(payload.Cards);
                    RunDataController.AddData("ShanliangDengpao", cards);
                    break;
            }
            StationPatch.RewardListener = null;
        }
    }

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

    [HarmonyPatch(typeof(ShopStation))]
    class ShopStationPatch
    {
        private static string Listener;
        private static int index = -1;

        [HarmonyPatch(nameof(ShopStation.OnEnter)), HarmonyPrefix]
        static void OnEnterPatch()
        {
            Listener = null;
            index = -1;
        }

        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterMapNode)), HarmonyPostfix, HarmonyPriority(1)]
        static void EnterMapNodePatch(GameRunController __instance)
        {
            Station CurrentStation = __instance.CurrentStation;
            if (!(CurrentStation is ShopStation station)) return;

            List<ShopItem<Card>> cardsList = station.ShopCards;
            List<CardWithPrice> cards = cardsList.Select(item =>
            {
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

        [HarmonyPatch(typeof(ShopStation))]
        class BuyCardPatch
        {
            private const string BuyCard = nameof(ShopStation.BuyCard);

            [HarmonyPatch(nameof(ShopStation.BuyCard))]
            static void Prefix()
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
                Listener = null;
            }
        }

        [HarmonyPatch(typeof(ShopStation))]
        class BuyExhibitRunnerPatch
        {
            private const string BuyExhibit = nameof(ShopStation.BuyExhibitRunner);

            [HarmonyPatch(nameof(ShopStation.BuyExhibitRunner))]
            static void Prefix()
            {
                Listener = BuyExhibit;
            }

            [HarmonyPatch(nameof(ShopStation.GetPrice), new Type[] { typeof(Exhibit) }), HarmonyPostfix]
            static void GetPricePatch(Exhibit exhibit, int __result)
            {
                if (Listener != BuyExhibit) return;
                RunDataController.CurrentStation.Rewards.TryGetValue("Exhibits", out object exhibits);
                RunDataController.AddPrice(exhibit.Id, __result);
                (exhibits as List<string>).Add(exhibit.Id);
                Listener = null;
            }
        }
    }
}