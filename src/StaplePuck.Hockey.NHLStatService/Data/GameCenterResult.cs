using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class GameCenterResult
    {
        public int id { get; set; }
        public int season { get; set; }
        public int gameType { get; set; }
        public string gameDate { get; set; } = string.Empty;
        public string gameState { get; set; } = string.Empty;
        public string gameScheduleState { get; set; } = string.Empty;
        public Team awayTeam { get; set; } = new Team();
        public Team homeTeam { get; set; } = new Team();
        public bool shootoutInUse { get; set; }
        public int maxPeriods { get; set; }
        public int regPeriods { get; set; }
        public bool otInUse { get; set; }
        public bool tiesInUse { get; set; }
        public Summary summary { get; set; } = new Summary();

        public class Team
        {
            public int id { get; set; }
            public string abbrev { get; set; } = string.Empty;
            public int score { get; set; }
            public int sog { get; set; }
        }

        public class Summary
        {
            public Linescore linescore { get; set; } = new Linescore();
            public Scoring[] scoring { get; set; } = new Scoring[0];
            public Shootout[] shootout { get; set; } = new Shootout[0];
            public Threestar[] threeStars { get; set; } = new Threestar[0];
            public Teamgamestat[] teamGameStats { get; set; } = new Teamgamestat[0];
            public Shotsbyperiod[] shotsByPeriod { get; set; } = new Shotsbyperiod[0];
            public Penalty[] penalties { get; set; } = new Penalty[0];
            public SeasonSeries[] seasonSeries { get; set; } = new SeasonSeries[0];
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

        public class Perioddescriptor
        {
            public int number { get; set; }
            public string periodType { get; set; } = string.Empty;
        }

        public class Scoring
        {
            public int period { get; set; }
            public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
            public Goal[] goals { get; set; } = new Goal[0];
        }

        public class Goal
        {
            public string situationCode { get; set; } = string.Empty;
            public string strength { get; set; } = string.Empty;
            public int playerId { get; set; }
            public Name firstName { get; set; } = new Name();
            public Name lastName { get; set; } = new Name();
            public Name name { get; set; } = new Name();
            public Name teamAbbrev { get; set; } = new Name();
            public string headshot { get; set; } = string.Empty;
            public long highlightClip { get; set; }
            public int goalsToDate { get; set; }
            public int awayScore { get; set; }
            public int homeScore { get; set; }
            public Name leadingTeamAbbrev { get; set; } = new Name();
            public string timeInPeriod { get; set; } = string.Empty;
            public string shotType { get; set; } = string.Empty;
            public string goalModifier { get; set; } = string.Empty;
            public Assist[] assists { get; set; } = new Assist[0];
        }

        public class Name
        {
            [JsonProperty(PropertyName = "default")]
            public string _default { get; set; } = string.Empty;
        }

        public class Assist
        {
            public int playerId { get; set; }
            public Name firstName { get; set; } = new Name();
            public Name lastName { get; set; } = new Name();
            public int assistsToDate { get; set; }
        }

        public class Shootout
        {
            public int sequence { get; set; }
            public int playerId { get; set; }
            public string teamAbbrev { get; set; } = string.Empty;
            public string firstName { get; set; } = string.Empty;
            public string lastName { get; set; } = string.Empty;
            public string shotType { get; set; } = string.Empty;
            public string result { get; set; } = string.Empty;
            public string headshot { get; set; } = string.Empty;
            public bool gameWinner { get; set; }
        }
        public class Threestar
        {
            public int star { get; set; }
            public int playerId { get; set; }
            public string teamAbbrev { get; set; } = string.Empty;
            public string headshot { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string firstName { get; set; } = string.Empty;
            public string lastName { get; set; } = string.Empty;
            public int sweaterNo { get; set; }
            public string position { get; set; } = string.Empty;
            public int goals { get; set; }
            public int assists { get; set; }
            public int points { get; set; }
        }

        public class Teamgamestat
        {
            public string category { get; set; } = string.Empty;
            public string awayValue { get; set; } = string.Empty;
            public string homeValue { get; set; } = string.Empty;
        }

        public class Shotsbyperiod
        {
            public int period { get; set; }
            public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Penalty
        {
            public int period { get; set; }
            public Perioddescriptor periodDescriptor { get; set; } = new Perioddescriptor();
            public Penalty1[] penalties { get; set; } = new Penalty1[0];
        }

        public class Penalty1
        {
            public string timeInPeriod { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
            public int duration { get; set; }
            public string committedByPlayer { get; set; } = string.Empty;
            public string teamAbbrev { get; set; } = string.Empty;
            public string drawnBy { get; set; } = string.Empty;
            public string descKey { get; set; } = string.Empty;
        }

        public class SeasonSeries
        {
            public int id { get; set; }
            public int season { get; set; }
            public int gameType { get; set; }
            public string gameDate { get; set; } = string.Empty;
            public string gameState { get; set; } = string.Empty;
            public string gameScheduleState { get; set; } = string.Empty;
            public Team awayTeam { get; set; } = new Team();
            public Team homeTeam { get; set; } = new Team();
            public int period { get; set; }
        }

        //public class Awayteam2
        //{
        //    public int id { get; set; }
        //    public string abbrev { get; set; } = string.Empty;
        //    public string logo { get; set; } = string.Empty;
        //    public int score { get; set; }
        //}

        //public class Hometeam2
        //{
        //    public int id { get; set; }
        //    public string abbrev { get; set; } = string.Empty;
        //    public string logo { get; set; } = string.Empty;
        //    public int score { get; set; }
        //}

        //public class Clock1
        //{
        //    public string timeRemaining { get; set; } = string.Empty;
        //    public int secondsRemaining { get; set; }
        //    public bool running { get; set; }
        //    public bool inIntermission { get; set; }
        //}

        //public class Gameoutcome
        //{
        //    public string lastPeriodType { get; set; } = string.Empty;
        //}
    }
}
