using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class BracketResult
    {
        public Series[] series { get; set; } = Array.Empty<Series>();

        public class Series
        {
            public int playoffRound { get; set; }
            public int winningTeamId { get; set; }
            public int losingTeamId { get; set; }
            public Seedteam topSeedTeam { get; set; } = new Seedteam();
            public Seedteam bottomSeedTeam { get; set; } = new Seedteam();
        }

        public class Seedteam
        {
            public int id { get; set; }
        }
    }
}
