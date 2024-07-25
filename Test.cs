using HarmonyLib;
using LBoL.Core.Dialogs;
using System.Diagnostics;

namespace RunLogger
{
    [HarmonyDebug]
    [HarmonyPatch(typeof(DialogFunctions), nameof(DialogFunctions.AdventureRand))]
    class DialogFunctions_Patch
    {
        static void Postfix(int a, int b, int __result)
        {
            BepinexPlugin.log.LogDebug($"adv rand command a:{a}, b:{b}, rez:{__result}");
        }
    }
}
