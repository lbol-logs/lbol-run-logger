using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.EntityLib.JadeBoxes;
using RunLogger.Utils;
using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyPatch]
    public static class CardChangePatch
    {
        [HarmonyPatch(typeof(Start50), nameof(Start50.OnGain))]
        public static class StartingDeckOverridePatch
        {
            static void Prefix()
            {
                Controller.Instance.IsOverridingStartingDeck = true;
            }

            static void Postfix(GameRunController gameRun)
            {
                Controller.Instance.StartingDeckOverride = gameRun._baseDeck;
            }
        }
    }
}