using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Request
{
    public class TeamStateForSeason
    {
        public Season? Season { get; set; }
        public Team? Team { get; set; }
        public int GameState { get; set; }
    }
}
