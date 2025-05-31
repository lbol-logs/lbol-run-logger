using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    public class RunLog
    {
        public string Version { get; internal set; }
#nullable enable
        public string? Name { get; internal set; }
#nullable disable
        public Settings Settings { get; internal set; }
        public List<StationObj> Stations { get; internal set; } = new List<StationObj>();
        public List<ActObj> Acts { get; internal set; } = new List<ActObj>();
        public List<CardChange> Cards { get; internal set; } = new List<CardChange>();
        public List<ExhibitChange> Exhibits { get; internal set; } = new List<ExhibitChange>();
        public Result Result { get; internal set; }
#nullable enable
        public string? Description { get; internal set; }
#nullable disable
    }
}