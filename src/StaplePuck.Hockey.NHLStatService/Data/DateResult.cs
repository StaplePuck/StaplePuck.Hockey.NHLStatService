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
        public Date[] dates { get; set; }
    }

    public class Date
    {
        public string date { get; set; }
        public int totalItems { get; set; }
        public int totalEvents { get; set; }
        public int totalGames { get; set; }
        public int totalMatches { get; set; }
        public Game[] games { get; set; }
        public object[] events { get; set; }
        public object[] matches { get; set; }
    }


    public class SeriesSummary
    { 
        public string seriesStatus { get; set; }
        public string seriesStatusShort { get; set; }
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
        public string link { get; set; }
        public string gameType { get; set; }
        public string season { get; set; }
        public DateTime gameDate { get; set; }
        public Status status { get; set; }
        public Teams teams { get; set; }
        public Decisions decisions { get; set; }
        public Scoringplay[] scoringPlays { get; set; }
        public SeriesSummary seriesSummary { get; set; }
    }

    public class Status
    {
        public string abstractGameState { get; set; }
        public string codedGameState { get; set; }
        public string detailedState { get; set; }
        public string statusCode { get; set; }
        public bool startTimeTBD { get; set; }

        public bool IsOver { get { return this.abstractGameState.Equals("Final", StringComparison.OrdinalIgnoreCase); } }
    }

    public class Teams
    {
        public Away away { get; set; }
        public Home home { get; set; }

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
        public Team team { get; set; }
    }

    public class Leaguerecord
    {
        public int wins { get; set; }
        public int losses { get; set; }
        public string type { get; set; }
    }

    public class Home
    {
        public int score { get; set; }
        public Team team { get; set; }
    }

    public class Decisions
    {
        public PlayerDecision winner { get; set; }
        public PlayerDecision loser { get; set; }
        public PlayerDecision firstStar { get; set; }
        public PlayerDecision secondStar { get; set; }
        public PlayerDecision thirdStar { get; set; }
    }

    public class PlayerDecision
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string link { get; set; }
    }

    public class Scoringplay
    {
        public Player[] players { get; set; }
        public Result result { get; set; }
        public About about { get; set; }
        public Team team { get; set; }
    }

    public class Result
    {
        public string _event { get; set; }
        public string eventCode { get; set; }
        public string eventTypeId { get; set; }
        public string description { get; set; }
        public string secondaryType { get; set; }
        public Strength strength { get; set; }
        public bool gameWinningGoal { get; set; }
        public bool emptyNet { get; set; }
    }

    public class About
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PeriodType periodType { get; set; }
        public string periodTime { get; set; }
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
        public string name { get; set; }
    }

    public class Goals
    {
        public int away { get; set; }
        public int home { get; set; }
    }

    public class Player
    {
        public Player1 player { get; set; }
        public string playerType { get; set; }
        public int seasonTotal { get; set; }
    }

    public class Player1
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public string link { get; set; }
    }

    public enum StrengthCode
    {
        EVEN,
        PPG,
        SHG
    }
}
