using RunLogger.Utils.RunLogLib.Nodes;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    internal class StationObj
    {
        internal string Type { get; set; }
        internal StationNode Node { get; set; }
        internal Status Status { get; set; }
        internal Dictionary<string, object> Data { get; set; }
#nullable enable
        internal string? Id { get; set; }
        internal Dictionary<string, object>? Rewards { get; set; }
#nullable disable
    }
}