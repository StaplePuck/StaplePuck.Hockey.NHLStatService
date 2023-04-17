using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService
{
    public class Settings
    {
        public string StatsUrlRoot { get; set; } = string.Empty;
        public string DateId { get; set; } = string.Empty;
        public bool Continuous { get; set; }
        public bool UpdateTeamState { get; set; }
        public int Delay { get; set; }
        public string SeasonId { get; set; } = string.Empty;
    }
}
