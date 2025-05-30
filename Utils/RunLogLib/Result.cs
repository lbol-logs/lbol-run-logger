﻿using RunLogger.Utils.RunLogLib.Entities;
using System.Collections.Generic;

namespace RunLogger.Utils.RunLogLib
{
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