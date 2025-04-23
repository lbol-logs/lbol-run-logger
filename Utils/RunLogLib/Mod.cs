using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class Mod
    {
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Dictionary<string, dynamic> Configs { get; set; }
    }
}