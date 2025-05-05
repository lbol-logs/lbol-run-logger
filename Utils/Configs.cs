namespace RunLogger.Utils
{
    internal static class Configs
    {
        internal const string RunLoggerDirName = "runLogger";
        internal const string TempDirName = "temp";

        internal const string RandomMana = "A";

        internal static readonly string[] PriorityMods = new string[] { PInfo.GUID, "neo.lbol.fix.rngFix" };
        internal static readonly string[] ExcludedMods = new string[] { "com.bepis.bepinex.scriptengine", "neo.lbol.tools.watermark" };
        internal const string HelpMeEirinMod = "neo.lbol.qol.helpMeEirin";
        internal const string PatchouliMod = "rmrfmaxx.lbol.PatchouliCharacterMod";
    }
}