using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    internal class Mod
    {
        internal string GUID { get; set; }
        internal string Name { get; set; }
        internal string Version { get; set; }
        internal Dictionary<string, dynamic> Configs { get; set; }
    }
}