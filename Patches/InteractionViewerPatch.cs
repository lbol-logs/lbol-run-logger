using HarmonyLib;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using RunLogger.Utils;
using System.Collections.Generic;

namespace RunLogger.Patches
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(InteractionViewer))]
    public class InteractionViewerPatch
    {
        [HarmonyPatch(nameof(InteractionViewer.View)), HarmonyPrefix]
        static void ViewPatch(Interaction interaction)
        {
            if (interaction is MiniSelectCardInteraction)
            {
                if (RunDataController.CurrentStationIndex == 0)
                {
                    MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;
                    Dictionary<string, object> Rewards = new Dictionary<string, object>
                    {
                        { "Cards", RunDataController.GetCards(cards) }
                    };
                    RunDataController.CurrentStation.Rewards = Rewards;
                }
            }

            if (interaction.Source == null) return;
            string source = interaction.Source.Id;

            if (interaction is RewardInteraction)
            {
                if (source == "HuiyeBaoxiang")
                {
                    RewardInteraction rewardInteraction = interaction as RewardInteraction;
                    IReadOnlyList<Exhibit> exhibits = rewardInteraction.PendingExhibits;
                    Debugger.Write(exhibits[0].Id);
                }
            }
            else if (interaction is MiniSelectCardInteraction)
            {
                if (source == "Modaoshu")
                {
                    MiniSelectCardInteraction miniSelectCardInteraction = interaction as MiniSelectCardInteraction;
                    IReadOnlyList<Card> cards = miniSelectCardInteraction.PendingCards;
                    Debugger.Write(cards[0].Id);
                }
            }
        }
    }
}
