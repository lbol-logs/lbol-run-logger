using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class Result
    {
        public string Type { get; internal set; }
        public string Timestamp { get; internal set; }
        public List<CardObj> Cards { get; internal set; }
        public List<string> Exhibits { get; internal set; }
        public string BaseMana { get; internal set; }
        public int ReloadTimes { get; internal set; }
        public string Seed { get; internal set; }
    }
}