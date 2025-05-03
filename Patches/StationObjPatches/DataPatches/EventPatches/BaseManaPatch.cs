using HarmonyLib;
using LBoL.Core.Adventures;
using LBoL.Core;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;
using System.Linq;

namespace RunLogger.Patches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class BaseManaPatch
    {
        private static readonly string[] Adventures = new[] { nameof(JunkoColorless), nameof(PatchouliPhilosophy) };

        [HarmonyPatch(typeof(Adventure), nameof(Debut.InitVariables)), HarmonyPostfix]
        private static void AddBaseMana(Adventure __instance)
        {
            if (!BaseManaPatch.Adventures.Contains(__instance.Id)) return;
            GameRunController gameRun = __instance.GameRun;
            string[] exhibits = gameRun.ExhibitRecord.ToArray();
            string baseMana = Helpers.GetBaseMana(gameRun.BaseMana.ToString(), exhibits);
            Helpers.AddDataValue("BaseMana", baseMana);
        }
    }
}