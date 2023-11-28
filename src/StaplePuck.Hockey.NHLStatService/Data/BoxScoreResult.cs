using Microsoft.Extensions.Diagnostics.HealthChecks;
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
        public string gameDate { get; set; } = string.Empty;
        public string gameState { get; set; } = string.Empty;
        public string gameScheduleState { get; set; } = string.Empty;
        public int period { get; set; }
        public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
        public Team awayTeam { get; set; } = new Team();
        public Team homeTeam { get; set; } = new Team();
        public Clock clock { get; set; } = new Clock();
        public Boxscore boxscore { get; set; } = new Boxscore();
        public Gameoutcome gameOutcome { get; set; } = new Gameoutcome();

        public class Perioddescriptor
        {
            public int number { get; set; }
            public string periodType { get; set; } = string.Empty;
        }

        public class Team
        {
            public int id { get; set; }
            public string abbrev { get; set; } = string.Empty;
            public int score { get; set; }
            public int sog { get; set; }
            public float faceoffWinningPctg { get; set; }
            public string powerPlayConversion { get; set; } = string.Empty;
            public int pim { get; set; }
            public int hits { get; set; }
            public int blocks { get; set; }
        }

        public class Clock
        {
            public string timeRemaining { get; set; } = string.Empty;
            public int secondsRemaining { get; set; }
            public bool running { get; set; }
            public bool inIntermission { get; set; }
        }

        public class Boxscore
        {
            public Linescore linescore { get; set; } = new Linescore();
            public Shotsbyperiod[] shotsByPeriod { get; set; } = new Shotsbyperiod[0];
            public Gamereports gameReports { get; set; } = new Gamereports();
            public Playerbygamestats playerByGameStats { get; set; } = new Playerbygamestats();
        }

        public class Linescore
        {
            public Byperiod[] byPeriod { get; set; } = new Byperiod[0];
            public Totals totals { get; set; } = new Totals();
        }

        public class Totals
        {
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Byperiod
        {
            public int period { get; set; }
            public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
            public int away { get; set; }
            public int home { get; set; }
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

        public class Playerbygamestats
        {
            public TeamPlayers awayTeam { get; set; } = new TeamPlayers();
            public TeamPlayers homeTeam { get; set; } = new TeamPlayers();
        }

        public class TeamPlayers
        {
            public PlayerStats[] forwards { get; set; } = new PlayerStats[0];
            public PlayerStats[] defense { get; set; } = new PlayerStats[0];
            public PlayerStats[] goalies { get; set; } = new PlayerStats[0];
        }

        public class PlayerStats
        {
            public int playerId { get; set; }
            public int sweaterNumber { get; set; }
            public Name name { get; set; } = new Name();
            public string position { get; set; } = string.Empty;
            public int goals { get; set; }
            public int assists { get; set; }
            public int points { get; set; }
            public int plusMinus { get; set; }
            public int pim { get; set; }
            public int hits { get; set; }
            public int blockedShots { get; set; }
            public int powerPlayGoals { get; set; }
            public int powerPlayPoints { get; set; }
            public int shorthandedGoals { get; set; }
            public int shPoints { get; set; }
            public int shots { get; set; }
            public string faceoffs { get; set; } = string.Empty;
            public float faceoffWinningPctg { get; set; }
            public string toi { get; set; } = string.Empty;
            public string powerPlayToi { get; set; } = string.Empty;
            public string shorthandedToi { get; set; } = string.Empty;
            
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

        public class Name
        {
            [JsonProperty(PropertyName = "default")]
            public string _default { get; set; } = string.Empty;
        }

        public class Shotsbyperiod
        {
            public int period { get; set; }
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
