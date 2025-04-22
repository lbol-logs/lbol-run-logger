using HarmonyLib;
using LBoL.Core;
using LBoL.EntityLib.JadeBoxes;

namespace RunLogger.Legacy.Patches
{
    [HarmonyPatch]
    public static class JadeBoxPatch
    {
        [HarmonyPatch(typeof(Start50), nameof(Start50.OnGain))]
        public static class Start50OnGainPatch
        {
            static void Prefix()
            {
                GameRunControllerPatch.isOverridingStartingDeck = true;
            }

            static void Postfix(GameRunController gameRun)
            {
                GameRunControllerPatch.startingDeckOverride = gameRun._baseDeck;
            }
        }
    }
}
