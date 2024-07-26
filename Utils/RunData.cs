using System.Collections.Generic;

namespace RunLogger.Utils
{
    public class RunData
    {
        public Info Info { get; set; } = new Info();
        public List<Station> Stations { get; set; } = new List<Station>();
        public List<StageObj> Stages { get; set; } = new List<StageObj>();
    }

    public class Info
    {
        public string PlayerType { get; set; }
        public bool HasClearBonus { get; set; }
        public bool ShowRandomResult { get; set; }
    }

    public class Station
    {
        public int Stage { get; set; }
        public int Level { get; set; }
        public string Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
#nullable enable
        public Dictionary<string, object>? Data { get; set; }
#nullable disable
    }

    public class StageObj
    {
        public int Stage { get; set; }
        public List<Node> Nodes { get; set; } = new List<Node>();
        public string Boss { get; set; }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public List<int> Followers { get; set; } = new List<int>();
    }
}