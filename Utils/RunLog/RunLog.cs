using RunLogger.Utils.RunLog.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLog
{
    public class RunLog
    {
        public string Version { get; set; }
#nullable enable
        public string? Name { get; set; }
#nullable disable
        public Settings Settings { get; set; }
        public List<StationObj> Stations { get; set; } = new List<StationObj>();
        public List<ActObj> Acts { get; set; } = new List<ActObj>();
        public List<CardChange> Cards { get; set; } = new List<CardChange>();
        public List<ExhibitChange> Exhibits { get; set; } = new List<ExhibitChange>();
        public Result Result { get; set; }
    }
}