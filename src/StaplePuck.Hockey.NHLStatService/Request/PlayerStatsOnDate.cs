using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Request
{
    public class PlayerStatsOnDate
    {
        public string GameDateId { get; init; } = string.Empty;
        public Player Player { get; set; } = new Player();
        public List<PlayerScore> PlayerScores { get; set; } = new List<PlayerScore>();
    }
}
