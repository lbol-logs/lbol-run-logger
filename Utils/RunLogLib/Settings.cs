using RunLogger.Utils.RunLogLib;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class Settings
    {
        public string Character { get; set; }
        public string PlayerType { get; set; }
        public bool HasClearBonus { get; set; }
        public bool ShowRandomResult { get; set; }
        public bool IsAutoSeed { get; set; }
        public string Difficulty { get; set; }
        public List<string> Requests { get; set; }
#nullable enable
        public List<string>? JadeBoxes { get; set; }
#nullable disable
        public List<Mod> Mods { get; set; }
        public Status Status { get; set; }
    }
}