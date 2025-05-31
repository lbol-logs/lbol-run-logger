using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib.Nodes
{
    public class ActNode
    {
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public List<int> Followers { get; internal set; } = new List<int>();
        public string Type { get; internal set; }
    }
}