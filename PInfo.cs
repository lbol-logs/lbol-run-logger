﻿using HarmonyLib;

namespace RunLogger
{
    public static class PInfo
    {
        public const string GUID = "ev.lbol.utils.runLogger";
        public const string Name = "Run Logger";
        public const string version = "3.2.2";
        public static readonly Harmony harmony = new Harmony(GUID);
    }
}