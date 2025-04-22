using System.Collections.Generic;

namespace RunLogger.Legacy.Utils
{
    public class RunData
    {
        public string Version { get; set; }
#nullable enable
        public string? Name { get; set; }
#nullable disable
        public Settings Settings { get; set; }
        public List<StationObj> Stations { get; set; } = new List<StationObj>();
        public List<ActObj> Acts { get; set; } = new List<ActObj>();
        public List<CardChange> Cards { get; set; } = new List<CardChange> { };
        public List<ExhibitChange> Exhibits { get; set; } = new List<ExhibitChange> { };
        public Result Result { get; set; }
    }

    public class Settings
    {
        public string Character { get; set; }
        public string PlayerType { get; set; }
        public bool HasClearBonus { get; set; }
        public bool ShowRandomResult { get; set; }
        public bool IsAutoSeed { get; set; }
        public string Difficulty { get; set; }
        public List<string> Requests { get; set; }
#nullable enable
        public List<string>? JadeBoxes { get; set; }
#nullable disable
        public List<Mod> Mods { get; set; }
        public Status Status { get; set; }
    }

    public class Mod
    {
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public Dictionary<string, dynamic> Configs { get; set; }
    }

    public class StationObj
    {
        public string Type { get; set; }
        public NodeObj Node { get; set; }
        public Status Status { get; set; }
        public Dictionary<string, object> Data { get; set; }
#nullable enable
        public string? Id { get; set; }
        public Dictionary<string, object>? Rewards { get; set; }
#nullable disable
    }

    public class NodeObj
    {
        public int Act { get; set; }
        public int Level { get; set; }
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
        public string Id { get; set; }
        public bool IsUpgraded { get; set; }
        public int? UpgradeCounter { get; set; }
    }

    public class CardWithPrice : CardObj
    {
        public int? Price { get; set; }
        public bool? IsDiscounted { get; set; }
    }

    public class ActObj
    {
        public int Act { get; set; }
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

    public class TurnObj
    {
        public int Round { get; set; }
        public int Turn { get; set; }
        public string Id { get; set; }
#nullable enable
        public List<CardObj>? Cards { get; set; }
        public List<IntentionObj>? Intentions { get; set; }
#nullable disable
        public BattleStatusObj Status { get; set; }
        public List<StatusEffectObj> StatusEffects { get; set; }
    }

    public class IntentionObj
    {
        public string Type { get; set; }
        public int? Damage { get; set; }
        public int? Times { get; set; }
        public bool? IsAccurate { get; set; }
    }

    public class BattleStatusObj
    {
        public int Hp { get; set; }
        public int Block { get; set; }
        public int Barrier { get; set; }
        public int? Power { get; set; }
    }

    public class StatusEffectObj
    {
        public string Id { get; set; }
        public int? Level { get; set; }
        public int? Duration { get; set; }
        public int? Count { get; set; }
        public int? Limit { get; set; }
    }

    public class CardChange : CardObj
    {
        public string Type { get; set; }
        public int Station { get; set; }
    }

    public class ExhibitChange
    {
        public string Id { get; set; }
        public int? Counter { get; set; }
        public string Type { get; set; }
        public int Station { get; set; }
    }

    public enum ChangeType
    {
        Add,
        Remove,
        Upgrade,
        Use
    }

    public class Result
    {
        public string Type { get; set; }
        public string Timestamp { get; set; }
        public List<CardObj> Cards { get; set; }
        public List<string> Exhibits { get; set; }
        public string BaseMana { get; set; }
        public int ReloadTimes { get; set; }
        public string Seed { get; set; }
    }
}