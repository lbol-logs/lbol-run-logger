using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib.Nodes
{
    internal class ActNode
    {
        internal int X { get; set; }
        internal int Y { get; set; }
        internal List<int> Followers { get; set; } = new List<int>();
        internal string Type { get; set; }
    }
}