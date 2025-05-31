using RunLogger.Utils.RunLogLib.Nodes;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class StationObj
    {
        public string Type { get; internal set; }
        public StationNode Node { get; internal set; }
        public Status Status { get; internal set; }
        public Dictionary<string, object> Data { get; internal set; }
#nullable enable
        public string? Id { get; internal set; }
        public Dictionary<string, object>? Rewards { get; internal set; }
#nullable disable
    }
}