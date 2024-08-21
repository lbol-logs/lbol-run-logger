using HarmonyLib;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.Interactions;
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
            Debugger.Write("view");
            if (!(interaction is RewardInteraction)) return;
            RewardInteraction rewardInteraction = interaction as RewardInteraction;
            List<Exhibit> exhibits = rewardInteraction.PendingExhibits.ToList();
            Debugger.Write(exhibits[0].Id);
            Debugger.Write(interaction.Source.Id);
        }

        //[HarmonyPatch("OnGain"), HarmonyPostfix]
        //static void OnGainPatch(Exhibit __instance)
        //{
        //    string exhibit = __instance.Id;
        //    switch (exhibit)
        //    {
        //        case "Modaoshu":
        //            RunDataController.isListening = true;
        //            break;
        //    }
        //}
    }
}
