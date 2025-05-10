using HarmonyLib;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core;
using System.Collections.Generic;
using RunLogger.Utils.RunLogLib.Entities;
using System.Linq;
using RunLogger.Utils;
using System;
using LBoL.EntityLib.Exhibits;

namespace RunLogger.Patches.StationObjPatches.DataPatches
{
    [HarmonyPatch]
    internal static class ShopPatch
    {
        [HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterMapNode)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
        private static void AddData(bool forced, GameRunController __instance)
        {
            if (!(__instance.CurrentStation is ShopStation station)) return;

            List<CardObjWithPrice> cardObjs = station.ShopCards.Select(item => Helpers.ParseCardWithPrice(item.Content, item.Price)).ToList();
            cardObjs[station.DiscountCardNo].IsDiscounted = true;

            Dictionary<string, int> prices = new Dictionary<string, int>()
            {
                { "Remove", station.RemoveDeckCardPrice },
                { "Upgrade", station.UpgradeDeckCardPrice }
            };

            List<ShopItem<Exhibit>> shopExhibits = station.ShopExhibits;
            List<string> exhibits = new List<string>();
            foreach (ShopItem<Exhibit> shopExhibit in shopExhibits)
            {
                string id = shopExhibit.Content.Id;
                prices[id] = shopExhibit.Price;
                exhibits.Add(id);
            }

            Dictionary<string, object> rewards = new Dictionary<string, object>()
            {
                { "Cards", new List<List<CardObjWithPrice>>() { cardObjs } },
                { "Exhibits", exhibits }
            };

            Controller.CurrentStation.Rewards = rewards;
            if (forced)
            {
                Helpers.GetData(out Dictionary<string, object> data);
                data["Prices"] = prices;
            }
            else
            {
                Helpers.AddDataValue("Prices", prices);
            }
        }

        [HarmonyPatch(typeof(ShopStation), nameof(ShopStation.RemoveDeckCard)), HarmonyPrefix]
        private static void AddRemove(Card card)
        {
            Helpers.AddDataValue("Choice", 0);
            Helpers.AddDataValue("Remove", Helpers.ParseCard(card));
        }

        [HarmonyPatch(typeof(ShopStation), nameof(ShopStation.UpgradeDeckCard)), HarmonyPrefix]
        private static void AddUpgrade(Card card)
        {
            Helpers.AddDataValue("Choice", 1);
            Helpers.AddDataValue("Upgrade", Helpers.ParseCard(card));
        }

        [HarmonyPatch(typeof(ShopStation), nameof(ShopStation.GetPrice), new Type[] { typeof(Card), typeof(bool) }), HarmonyPostfix]
        private static void AddExtraCard(Card card, int __result, ShopStation __instance)
        {
            ShopStation shopStation = __instance;
            bool isOnEnter = ShopPatch.IsOnEnter(shopStation);
            if (isOnEnter) return;

            Controller.CurrentStation.Rewards.TryGetValue("Cards", out object value);
            List<List<CardObjWithPrice>> cards = value as List<List<CardObjWithPrice>>;

            bool isAppended = cards.Count > 1 && cards[1][0].Price != null;
            if (!isAppended) cards.Insert(1, new List<CardObjWithPrice>());
            List<CardObjWithPrice> cardObjs = cards[1];
            int price = ShopPatch.GetPrice(shopStation, __result);
            cardObjs.Add(Helpers.ParseCardWithPrice(card, price));
        }

        [HarmonyPatch(typeof(ShopStation), nameof(ShopStation.GetPrice), new Type[] { typeof(Exhibit) }), HarmonyPostfix]
        private static void AddExtraExhibit(Exhibit exhibit, int __result, ShopStation __instance)
        {
            if (exhibit is KongZhanpinhe) return;

            ShopStation shopStation = __instance;
            bool isOnEnter = IsOnEnter(shopStation);
            if (isOnEnter) return;

            Controller.CurrentStation.Rewards.TryGetValue("Exhibits", out object value);
            List<string> exhibits = value as List<string>;

            int price = ShopPatch.GetPrice(shopStation, __result);
            string id = exhibit.Id;
            exhibits.Add(id);
            Helpers.GetData(out Dictionary<string, object> data);
            data.TryGetValue("Prices", out object value2);
            Dictionary<string, int> prices = value2 as Dictionary<string, int>;
            prices.Add(id, price);
        }

        private static bool IsOnEnter(ShopStation shopStation)
        {
            return shopStation.ShopExhibits == null;
        }

        private static int GetPrice(ShopStation shopStation, int basePrice)
        {
            return (int)(shopStation.GameRun.FinalShopPriceMultiplier * basePrice);
        }
    }
}