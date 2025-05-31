using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class Settings
    {
        public string Character { get; internal set; }
        public string PlayerType { get; internal set; }
        public bool HasClearBonus { get; internal set; }
        public bool ShowRandomResult { get; internal set; }
        public bool IsAutoSeed { get; internal set; }
        public string Difficulty { get; internal set; }
        public List<string> Requests { get; internal set; }
#nullable enable
        public List<string>? JadeBoxes { get; internal set; }
#nullable disable
        public List<Mod> Mods { get; internal set; }
        public Status Status { get; internal set; }
    }
}