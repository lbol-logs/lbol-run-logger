using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    internal class Settings
    {
        internal string Character { get; set; }
        internal string PlayerType { get; set; }
        internal bool HasClearBonus { get; set; }
        internal bool ShowRandomResult { get; set; }
        internal bool IsAutoSeed { get; set; }
        internal string Difficulty { get; set; }
        internal List<string> Requests { get; set; }
#nullable enable
        internal List<string>? JadeBoxes { get; set; }
#nullable disable
        internal List<Mod> Mods { get; set; }
        internal Status Status { get; set; }
    }
}