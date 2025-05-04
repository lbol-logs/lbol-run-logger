using HarmonyLib;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.Core;
using LBoL.Core.Adventures;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.Dialogs;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.FirstPlace;
using LBoL.EntityLib.Adventures.Shared12;
using LBoL.EntityLib.Adventures.Shared23;
using LBoL.EntityLib.Adventures.Stage1;
using LBoL.EntityLib.Adventures.Stage2;
using LBoL.EntityLib.Adventures.Stage3;
using LBoL.EntityLib.Exhibits.Adventure;
using LBoL.Presentation.UI.Panels;
using RunLogger.Legacy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch(typeof(Adventure))]
    public static class AdventurePatch
    {
        //[HarmonyPatch(typeof(EternityAscension))]
        //public static class EternityAscensionPatch
        //{
        //    [HarmonyPatch(nameof(EternityAscension.InitVariables))]
        //    public static void Postfix(EternityAscension __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$transformCard", out string transformCard);
        //            RunDataController.AddData("Card", transformCard);
        //        }
        //    }
        //}

        //[HarmonyPatch(typeof(KaguyaVersusMokou))]
        //public static class KaguyaVersusMokouPatch
        //{
        //    [HarmonyPatch(nameof(KaguyaVersusMokou.InitVariables))]
        //    public static void Postfix(KaguyaVersusMokou __instance)
        //    {
        //        __instance.Storage.TryGetValue("$hpLose", out float hpLose);
        //        __instance.Storage.TryGetValue("$hpLoseLow", out float hpLoseLow);
        //        RunDataController.AddData("Hps", new[] { (int)hpLose, (int)hpLoseLow });
        //    }
        //}

        //[HarmonyPatch(typeof(ParseeJealousy))]
        //public static class ParseeJealousyPatch
        //{
        //    [HarmonyPatch(nameof(ParseeJealousy.InitVariables))]
        //    public static void Postfix(ParseeJealousy __instance)
        //    {
        //        if (RunDataController.ShowRandom)
        //        {
        //            __instance.Storage.TryGetValue("$exhibitPassBy", out string exhibitPassBy);
        //            RunDataController.AddData("Exhibit", exhibitPassBy);
        //        }
        //    }

        //    [HarmonyPatch(nameof(ParseeJealousy.GetExhibit)), HarmonyPostfix]
        //    public static void GetExhibitPatch(ParseeJealousy __instance)
        //    {
        //        __instance.Storage.TryGetValue("$exhibit", out string exhibit);
        //        __instance.Storage.TryGetValue("$exhibit2", out string exhibit2);
        //        RunDataController.AddData("Exhibits", new[] { exhibit, exhibit2 });
        //    }
        //}
    }
}