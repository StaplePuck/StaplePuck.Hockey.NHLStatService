using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService
{
    public class Settings
    {
        public string StatsUrlRoot { get; set; }

        public string DateId { get; set; }
        public bool Continuous { get; set; }
        public bool UpdateTeamState { get; set; }
        public int Delay { get; set; }
        public string SeasonId { get; set; }
    }
}
