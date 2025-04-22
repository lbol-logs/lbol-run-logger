using System.Collections.Generic;
using RunLogger.Utils.RunLog.Nodes;

namespace RunLogger.Utils.RunLog
{
    public class ActObj
    {
        public int Act { get; set; }
        public List<ActNode> Nodes { get; set; } = new List<ActNode>();
        public string Boss { get; set; }
    }
}