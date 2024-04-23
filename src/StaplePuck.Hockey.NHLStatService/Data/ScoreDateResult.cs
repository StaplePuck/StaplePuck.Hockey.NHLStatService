namespace StaplePuck.Hockey.NHLStatService.Data
{
    public class ScoreDateResult
    {
        public string prevDate { get; set; } = string.Empty;
        public string currentDate { get; set; } = string.Empty;
        public string nextDate { get; set; } = string.Empty;
        public Game[] games { get; set; } = new Game[0];

        public class Game
        {
            public int id { get; set; }
            public int season { get; set; }
            public int gameType { get; set; }
            public string gameState { get; set; } = string.Empty;
            public Team awayTeam { get; set; } = new Team();
            public Team homeTeam { get; set; } = new Team();
            public SeriesStatus seriesStatus { get; set; } = new SeriesStatus();
        }

        public class Team
        {
            public int id { get; set; }
            public string abbrev { get; set; } = string.Empty;
            public int score { get; set; }
            public int sog { get; set; }
            public string logo { get; set; } = string.Empty;
        }


        public class SeriesStatus
        {
            public int round { get; set; }
            public string seriesAbbrev { get; set; } = string.Empty;
            public string seriesLetter { get; set; } = string.Empty;
            public int neededToWin { get; set; }
            public string topSeedTeamAbbrev { get; set; } = string.Empty;
            public int topSeedWins { get; set; }
            public string bottomSeedTeamAbbrev { get; set; } = string.Empty;
            public int bottomSeedWins { get; set; }
            public int gameNumberOfSeries { get; set; }
        }

    }
}
