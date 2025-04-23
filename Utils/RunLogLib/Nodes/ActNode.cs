using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib.Nodes
{
    public class ActNode
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<int> Followers { get; set; } = new List<int>();
        public string Type { get; set; }
    }
}