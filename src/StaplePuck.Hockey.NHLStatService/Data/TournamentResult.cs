using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class TournamentResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public string season { get; set; }
        public Round[] rounds { get; set; }
    }

    public class Round
    {
        public int number { get; set; }
        public int code { get; set; }
        public Names names { get; set; }
        public Format format { get; set; }
        public Series[] series { get; set; }
    }

    public class Names
    {
        public string name { get; set; }
        public string shortName { get; set; }
    }

    public class Format
    {
        public string name { get; set; }
        public string description { get; set; }
        public int numberOfGames { get; set; }
        public int numberOfWins { get; set; }
    }

    public class Series
    {
        public int seriesNumber { get; set; }
        public string seriesCode { get; set; }
        public Names1 names { get; set; }
        public Currentgame currentGame { get; set; }
        public Conference conference { get; set; }
        public Round1 round { get; set; }
        public Matchupteam[] matchupTeams { get; set; }
    }

    public class Names1
    {
        public string matchupName { get; set; }
        public string matchupShortName { get; set; }
        public string teamAbbreviationA { get; set; }
        public string teamAbbreviationB { get; set; }
        public string seriesSlug { get; set; }
    }

    public class Currentgame
    {
        public Seriessummary seriesSummary { get; set; }
    }

    public class Seriessummary
    {
        public int gamePk { get; set; }
        public int gameNumber { get; set; }
        public string gameLabel { get; set; }
        public bool necessary { get; set; }
        public int gameCode { get; set; }
        public DateTime gameTime { get; set; }
        public string seriesStatus { get; set; }
        public string seriesStatusShort { get; set; }
    }

    public class Conference
    {
        public int id { get; set; }
        public string name { get; set; }
        public string link { get; set; }
    }

    public class Round1
    {
        public int number { get; set; }
    }

    public class Matchupteam
    {
        public Team team { get; set; }
        public Seed seed { get; set; }
        public Seriesrecord seriesRecord { get; set; }
    }

    public class Seed
    {
        public string type { get; set; }
        public int rank { get; set; }
        public bool isTop { get; set; }
    }

    public class Seriesrecord
    {
        public int wins { get; set; }
        public int losses { get; set; }
    }

}
