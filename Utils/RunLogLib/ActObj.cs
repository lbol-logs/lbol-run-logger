using System.Collections.Generic;
using RunLogger.Utils.RunLogLib.Nodes;

namespace RunLogger.Utils.RunLogLib
{
    internal class ActObj
    {
        internal int Act { get; set; }
        internal List<ActNode> Nodes { get; set; } = new List<ActNode>();
        internal string Boss { get; set; }
    }
}