using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class Team
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
    }
}
