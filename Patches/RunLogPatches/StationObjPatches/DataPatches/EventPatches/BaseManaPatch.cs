using HarmonyLib;
using LBoL.Core.Adventures;
using LBoL.Core;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Adventures.FirstPlace;
using RunLogger.Utils;
using System.Linq;
using LBoL.Base;
using LBoL.Core.Battle.Interactions;
using System.Collections.Generic;

namespace RunLogger.Patches.RunLogPatches.StationObjPatches.DataPatches.EventPatches
{
    [HarmonyPatch]
    internal static class BaseManaPatch
    {
        private static readonly string[] Adventures = new[] { nameof(JunkoColorless), nameof(PatchouliPhilosophy) };

        [HarmonyPatch(typeof(Adventure), nameof(Debut.InitVariables)), HarmonyPostfix]
        private static void AddBaseMana(Adventure __instance)
        {
            if (!Instance.IsInitialized) return;

            if (!BaseManaPatch.Adventures.Contains(__instance.Id)) return;
            GameRunController gameRun = __instance.GameRun;
            string[] exhibits = gameRun.ExhibitRecord.ToArray();
            string baseMana = Helpers.GetBaseMana(gameRun.BaseMana.ToString(), exhibits);
            Helpers.AddDataValue("BaseMana", baseMana);
        }

        [HarmonyPatch(typeof(SelectBaseManaInteraction), nameof(SelectBaseManaInteraction.SelectedMana), MethodType.Setter)]
        private static class AddColor
        {
            private static void Prefix(ManaGroup value)
            {
                if (!Instance.IsInitialized) return;

                ManaGroup mana = value;
                string color = mana.MaxColor.ToShortName().ToString();
                Helpers.AddDataValue("Color", color);
            }

            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                return instructions;
            }
        }
    }
}