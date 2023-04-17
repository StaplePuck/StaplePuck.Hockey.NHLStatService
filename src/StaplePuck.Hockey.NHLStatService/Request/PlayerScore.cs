using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Request
{
    public class PlayerScore
    {
        public bool AdminOverride { get; set; }
        public ScoringType ScoringType { get; set; } = new ScoringType();
        public int Total { get; set; }
    }
}
