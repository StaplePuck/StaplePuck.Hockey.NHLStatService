using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class DateResult
    {
        public Date[] dates { get; set; } = Array.Empty<Date>();
    }

    public class Date
    {
        public string date { get; set; } = string.Empty;
        public int totalItems { get; set; }
        public int totalEvents { get; set; }
        public int totalGames { get; set; }
        public int totalMatches { get; set; }
        public Game[] games { get; set; } = Array.Empty<Game>();
    }


    public class SeriesSummary
    { 
        public string seriesStatus { get; set; } = string.Empty;
        public string seriesStatusShort { get; set; } = string.Empty;
        public bool IsOver
        {
            get
            {
                return seriesStatus.Contains(" win ");
            }
        }

    }


    public class Game
    {
        public int gamePk { get; set; }
        public string link { get; set; } = string.Empty;
        public string gameType { get; set; } = string.Empty;
        public string season { get; set; } = string.Empty;
        public DateTime? gameDate { get; set; }
        public Status? status { get; set; }
        public Teams? teams { get; set; }
        public Decisions? decisions { get; set; }
        public Scoringplay[] scoringPlays { get; set; } = Array.Empty<Scoringplay>();
        public SeriesSummary? seriesSummary { get; set; }
    }

    public class Status
    {
        public string abstractGameState { get; set; } = string.Empty;
        public string codedGameState { get; set; } = string.Empty;
        public string detailedState { get; set; } = string.Empty;
        public string statusCode { get; set; } = string.Empty;
        public bool startTimeTBD { get; set; }

        public bool IsOver { get { return this.abstractGameState.Equals("Final", StringComparison.OrdinalIgnoreCase); } }
    }

    public class Teams
    {
        public Away away { get; set; } = new Away();
        public Home home { get; set; } = new Home();

        public int LosingScore
        {
            get
            {
                if (away.score < home.score)
                {
                    return away.score;
                }
                return home.score;
            }
        }

        public int WinningScore
        {
            get
            {
                if (away.score > home.score)
                {
                    return away.score;
                }
                return home.score;
            }
        }
    }

    public class Away
    {
        public int score { get; set; }
        public Team? team { get; set; }
    }

    public class Leaguerecord
    {
        public int wins { get; set; }
        public int losses { get; set; }
        public string type { get; set; } = string.Empty;
    }

    public class Home
    {
        public int score { get; set; }
        public Team? team { get; set; }
    }

    public class Decisions
    {
        public PlayerDecision? winner { get; set; }
        public PlayerDecision? loser { get; set; }
        public PlayerDecision? firstStar { get; set; }
        public PlayerDecision? secondStar { get; set; }
        public PlayerDecision? thirdStar { get; set; }
    }

    public class PlayerDecision
    {
        public int id { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
    }

    public class Scoringplay
    {
        public Player[] players { get; set; } = Array.Empty<Player>();
        public Result? result { get; set; }
        public About? about { get; set; }
        public Team? team { get; set; }
    }

    public class Result
    {
        public string _event { get; set; } = string.Empty;
        public string eventCode { get; set; } = string.Empty;
        public string eventTypeId { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string secondaryType { get; set; } = string.Empty;
        public Strength? strength { get; set; }
        public bool gameWinningGoal { get; set; }
        public bool emptyNet { get; set; }
    }

    public class About
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PeriodType periodType { get; set; }
        public string periodTime { get; set; } = string.Empty;
    }

    public enum PeriodType
    {
        REGULAR,
        OVERTIME,
        SHOOTOUT
    }

    public class Strength
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public StrengthCode code { get; set; }
        public string name { get; set; } = string.Empty;
    }

    public class Goals
    {
        public int away { get; set; }
        public int home { get; set; }
    }

    public class Player
    {
        public Player1? player { get; set; }
        public string playerType { get; set; } = string.Empty;
        public int seasonTotal { get; set; }
    }

    public class Player1
    {
        public int id { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
    }

    public enum StrengthCode
    {
        EVEN,
        PPG,
        SHG
    }
}
