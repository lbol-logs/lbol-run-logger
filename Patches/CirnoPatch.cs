using HarmonyLib;
using RunLogger.Utils;
using LBoL.EntityLib.EnemyUnits.Opponent;
using System;

namespace RunLogger.Patches
{
    [HarmonyPatch(typeof(Cirno))]
    public static class CirnoPatch
    {
        [HarmonyPatch(nameof(Cirno.SetNextBuff)), HarmonyPrefix]
        static void SetNextBuffPatch(Cirno __instance)
        {
            Type buff = __instance.NextBuff;
            if (buff == null) return;

            string id = buff.Name;
            int level = __instance.NextLevel;
            StatusEffectObj statusEffect = new StatusEffectObj()
            {
                Id = id,
                Level = level
            };
            RunDataController.AddDataItem("Teammates", statusEffect);
        }
    }
}
