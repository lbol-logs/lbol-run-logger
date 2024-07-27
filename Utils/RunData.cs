﻿using System.Collections.Generic;

namespace RunLogger.Utils
{
    public class RunData
    {
        public Settings Settings { get; set; }
        public List<StationObj> Stations { get; set; } = new List<StationObj>();
        public List<StageObj> Stages { get; set; } = new List<StageObj>();
        public string Result { get; set; }
        public string Timestamp { get; set; }
    }

    public class Settings
    {
        public string Character { get; set; }
        public string PlayerType { get; set; }
        public bool HasClearBonus { get; set; }
        public bool ShowRandomResult { get; set; }
        public bool IsAutoSeed { get; set; }
        public List<string> Puzzles { get; set; }
        public string Difficulty { get; set; }
        public Status Status { get; set; }
    }

    public class StationObj
    {
        public string Type { get; set; }
        public Position Position { get; set; }
        public Status Status { get; set; }
        public Dictionary<string, object> Data { get; set; }
#nullable enable
        public string? Name { get; set; }
        public Dictionary<string, object>? Rewards { get; set; }
#nullable disable
    }

    public class Position
    {
        public int Stage { get; set; }
        public int Level { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Status
    {
        public int Money { get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Power { get; set; }
        public int MaxPower { get; set; }
    }

    public class CardObj
    {
        public string Name { get; set; }
        public bool IsUpgraded { get; set; }
        public int? UpgradeCounter { get; set; }
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
        public string Type { get; set; }
    }
}