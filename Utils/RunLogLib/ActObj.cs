using System.Collections.Generic;
using RunLogger.Utils.RunLogLib.Nodes;

namespace RunLogger.Utils.RunLogLib
{
    public class ActObj
    {
        public int Act { get; set; }
        public List<ActNode> Nodes { get; set; } = new List<ActNode>();
        public string Boss { get; set; }
    }
}