using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class TournamentResult
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string season { get; set; } = string.Empty;
        public Round[] rounds { get; set; } = Array.Empty<Round>();
    }

    public class Round
    {
        public int number { get; set; }
        public int code { get; set; }
        public Names? names { get; set; }
        public Format? format { get; set; }
        public Series[] series { get; set; } = Array.Empty<Series>();
    }

    public class Names
    {
        public string name { get; set; } = string.Empty;
        public string shortName { get; set; } = string.Empty;
    }

    public class Format
    {
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int numberOfGames { get; set; }
        public int numberOfWins { get; set; }
    }

    public class Series
    {
        public int seriesNumber { get; set; }
        public string seriesCode { get; set; } = string.Empty;
        public Names1? names { get; set; }
        public Currentgame? currentGame { get; set; }
        public Conference? conference { get; set; }
        public Round1? round { get; set; }
        public Matchupteam[] matchupTeams { get; set; } = Array.Empty<Matchupteam>();
    }

    public class Names1
    {
        public string matchupName { get; set; } = string.Empty;
        public string matchupShortName { get; set; } = string.Empty;
        public string teamAbbreviationA { get; set; } = string.Empty;
        public string teamAbbreviationB { get; set; } = string.Empty;
        public string seriesSlug { get; set; } = string.Empty;
    }

    public class Currentgame
    {
        public Seriessummary? seriesSummary { get; set; }
    }

    public class Seriessummary
    {
        public int gamePk { get; set; }
        public int gameNumber { get; set; }
        public string gameLabel { get; set; } = string.Empty;
        public bool necessary { get; set; }
        public int gameCode { get; set; }
        public DateTime? gameTime { get; set; }
        public string seriesStatus { get; set; } = string.Empty;
        public string seriesStatusShort { get; set; } = string.Empty;
    }

    public class Conference
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string link { get; set; } = string.Empty;
    }

    public class Round1
    {
        public int number { get; set; }
    }

    public class Matchupteam
    {
        public Team? team { get; set; }
        public Seed? seed { get; set; }
        public Seriesrecord? seriesRecord { get; set; }
    }

    public class Seed
    {
        public string type { get; set; } = string.Empty;
        public int rank { get; set; }
        public bool isTop { get; set; }
    }

    public class Seriesrecord
    {
        public int wins { get; set; }
        public int losses { get; set; }
    }

}
