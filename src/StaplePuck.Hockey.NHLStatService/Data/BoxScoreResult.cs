using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class BoxScoreResult
    {
        public int id { get; set; }
        public int season { get; set; }
        public int gameType { get; set; }
        public bool limitedScoring { get; set; }
        public string gameDate { get; set; } = string.Empty;
        public DateTime startTimeUTC { get; set; } = new DateTime();
        public string gameState { get; set; } = string.Empty;
        public string gameScheduleState { get; set; } = string.Empty;
        public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
        public int regPeriods { get; set; }
        public Team awayTeam { get; set; } = new Team();
        public Team homeTeam { get; set; } = new Team();
        public Playerbygamestats playerByGameStats { get; set; } = new Playerbygamestats();
        public Summary summary { get; set; } = new Summary();
        public Gameoutcome gameOutcome { get; set; } = new Gameoutcome();

        public class Perioddescriptor
        {
            public int number { get; set; }
            public string periodType { get; set; } = string.Empty;
        }

        public class Team
        {
            public int id { get; set; }
            public Name name { get; set; } = new Name();
            public string abbrev { get; set; } = string.Empty;
            public int score { get; set; }
            public int sog { get; set; }
            public string logo { get; set; } = string.Empty;
            public Name placeName { get; set; } = new Name();
        }

        public class Name
        {
            [JsonProperty(PropertyName = "default")]
            public string _default { get; set; } = string.Empty;
        }

        public class Playerbygamestats
        {
            public TeamPlayers awayTeam { get; set; } = new TeamPlayers();
            public TeamPlayers homeTeam { get; set; } = new TeamPlayers();
        }

        public class TeamPlayers
		{
            public PlayerStats[] forwards { get; set; } = Array.Empty<PlayerStats>();
            public PlayerStats[] defense { get; set; } = Array.Empty<PlayerStats>();
            public PlayerStats[] goalies { get; set; } = Array.Empty<PlayerStats>();
        }

        public class PlayerStats
		{
            public int playerId { get; set; }
            public int sweaterNumber { get; set; }
            public Name name { get; set; } = new Name();
            public string position { get; set; } = string.Empty;
            public int goals { get; set; }
            public int assists { get; set; }
            public int shorthandedGoals { get; set; }
			public int points { get; set; }
            public int plusMinus { get; set; }
            public int pim { get; set; }
            public int hits { get; set; }
            public int powerPlayGoals { get; set; }
            public int shots { get; set; }
            public float faceoffWinningPctg { get; set; }
            public string toi { get; set; } = string.Empty;

			public string evenStrengthShotsAgainst { get; set; } = string.Empty;
			public string powerPlayShotsAgainst { get; set; } = string.Empty;
			public string shorthandedShotsAgainst { get; set; } = string.Empty;
			public string saveShotsAgainst { get; set; } = string.Empty;
			public string savePctg { get; set; } = string.Empty;
			public int evenStrengthGoalsAgainst { get; set; }
			public int powerPlayGoalsAgainst { get; set; }
			public int shorthandedGoalsAgainst { get; set; }
			public int goalsAgainst { get; set; }
		}

        public class Summary
        {
            public Linescore linescore { get; set; } = new Linescore();
            public Shotsbyperiod[] shotsByPeriod { get; set; } = Array.Empty<Shotsbyperiod>();
            public Seasonserieswins seasonSeriesWins { get; set; } = new Seasonserieswins();
            public Gamereports gameReports { get; set; } = new Gamereports();
        }

        public class Linescore
        {
            public Byperiod[] byPeriod { get; set; } = Array.Empty<Byperiod>();
            public Totals totals { get; set; } = new Totals();
        }

        public class Totals
        {
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Byperiod
        {
            public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Seasonserieswins
        {
            public int awayTeamWins { get; set; }
            public int homeTeamWins { get; set; }
        }

        public class Gamereports
        {
            public string gameSummary { get; set; } = string.Empty;
            public string eventSummary { get; set; } = string.Empty;
            public string playByPlay { get; set; } = string.Empty;
            public string faceoffSummary { get; set; } = string.Empty;
            public string faceoffComparison { get; set; } = string.Empty;
            public string rosters { get; set; } = string.Empty;
            public string shotSummary { get; set; } = string.Empty;
            public string shiftChart { get; set; } = string.Empty;
            public string toiAway { get; set; } = string.Empty;
            public string toiHome { get; set; } = string.Empty;
        }

        public class Shotsbyperiod
        {
            public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Gameoutcome
        {
            public string lastPeriodType { get; set; } = string.Empty;
        }
    }
}
