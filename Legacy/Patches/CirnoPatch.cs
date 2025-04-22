using HarmonyLib;
using LBoL.EntityLib.EnemyUnits.Opponent;
using System;
using RunLogger.Legacy.Utils;

namespace RunLogger.Legacy.Patches
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
