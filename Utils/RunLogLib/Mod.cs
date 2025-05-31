using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class Mod
    {
        public string GUID { get; internal set; }
        public string Name { get; internal set; }
        public string Version { get; internal set; }
        public Dictionary<string, dynamic> Configs { get; internal set; }
    }
}