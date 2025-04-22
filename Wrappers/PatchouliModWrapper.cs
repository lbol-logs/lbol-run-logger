using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RunLogger.Wrappers
{
    internal class PatchouliModWrapper
    {
        internal static Dictionary<string, dynamic> GetConfigs() =>
            new Dictionary<string, dynamic>()
            {
                { "startingExhibitSign", GetStartingExhibitSignWrapper() },
                { "startingCardSign", GetStartingCardSignWrapper() }
            };

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static int GetStartingExhibitSignWrapper() => (int)PatchouliCharacterMod.Config.PatchouliConfigEntry.GetStartingExhibitSign();

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static int GetStartingCardSignWrapper() => (int)PatchouliCharacterMod.Config.PatchouliConfigEntry.GetStartingCardSign();
    }
}
