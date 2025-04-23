using RunLogger.Utils.RunLogLib.Nodes;
using RunLogger.Utils.RunLogLib;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class StationObj
    {
        public string Type { get; set; }
        public StationNode Node { get; set; }
        public Status Status { get; set; }
        public Dictionary<string, object> Data { get; set; }
#nullable enable
        public string? Id { get; set; }
        public Dictionary<string, object>? Rewards { get; set; }
#nullable disable
    }
}