using HarmonyLib;

namespace RunLogger
{
    public static class PInfo
    {
        // each loaded plugin needs to have a unique GUID. usually author+generalCategory+Name is good enough
        public const string GUID = "ev.lbol.utils.runLogger";
        public const string Name = "Run Logger";
        public const string version = "0.9.11";
        public static readonly Harmony harmony = new Harmony(GUID);
    }
}