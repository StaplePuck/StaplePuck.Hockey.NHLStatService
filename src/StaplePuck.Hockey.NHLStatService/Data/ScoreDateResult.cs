namespace StaplePuck.Hockey.NHLStatService.Data
{
    internal class ScoreDateResult
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
        }

        public class Team
        {
            public int id { get; set; }
            public string abbrev { get; set; } = string.Empty;
            public int score { get; set; }
            public int sog { get; set; }
            public string logo { get; set; } = string.Empty;
        }
    }    
}
