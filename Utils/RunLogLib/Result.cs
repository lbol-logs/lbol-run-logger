using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    internal class Result
    {
        internal string Type { get; set; }
        internal string Timestamp { get; set; }
        internal List<CardObj> Cards { get; set; }
        internal List<string> Exhibits { get; set; }
        internal string BaseMana { get; set; }
        internal int ReloadTimes { get; set; }
        internal string Seed { get; set; }
    }
}