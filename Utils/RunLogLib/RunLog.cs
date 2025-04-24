using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
    internal class RunLog
    {
        internal string Version { get; set; }
#nullable enable
        internal string? Name { get; set; }
#nullable disable
        internal Settings Settings { get; set; }
        internal List<StationObj> Stations { get; set; } = new List<StationObj>();
        internal List<ActObj> Acts { get; set; } = new List<ActObj>();
        internal List<CardChange> Cards { get; set; } = new List<CardChange>();
        internal List<ExhibitChange> Exhibits { get; set; } = new List<ExhibitChange>();
        internal Result Result { get; set; }
    }
}