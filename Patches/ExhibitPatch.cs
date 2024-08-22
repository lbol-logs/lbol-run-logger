using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using RunLogger.Utils;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(Exhibit))]
    class ExhibitPatch
    {
        [HarmonyPatch(typeof(InteractionViewer), nameof(InteractionViewer.View)), HarmonyPrefix]
        static void ViewPatch(Interaction interaction)
        {
            if (interaction.Source == null) return;
            string source = interaction.Source.Id;
            if (interaction is RewardInteraction)
            {
                if (source == "HuiyeBaoxiang")
                {
                    RewardInteraction rewardInteraction = interaction as RewardInteraction;
                    List<Exhibit> exhibits = rewardInteraction.PendingExhibits.ToList();
                    Debugger.Write(exhibits[0].Id);
                }
            }
            else if (interaction is MiniSelectCardInteraction)
            {
                if (source == "Modaoshu")
                {
                    MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
                    List<Card> cards = miniSelectCardInteraction.PendingCards.ToList();
                    Debugger.Write(cards[0].Id);
                }
            }
        }
    }
}
