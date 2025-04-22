using RunLogger.Utils.RunLog.Nodes;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLog
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