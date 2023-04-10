using StaplePuck.Core.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Request
{
    public class GameDate
    {
        public string Id { get; set; } = string.Empty;
        public List<PlayerStatsOnDate> PlayersStatsOnDate { get; set; } = new List<PlayerStatsOnDate>();
        public List<GameDateSeason> GameDateSeasons { get; set; } = new List<GameDateSeason>();
    }
}
