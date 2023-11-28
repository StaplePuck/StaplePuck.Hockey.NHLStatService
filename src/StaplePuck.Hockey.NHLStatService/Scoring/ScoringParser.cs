using StaplePuck.Hockey.NHLStatService.Data;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public abstract class ScoringParser : IScoringParser
    {
        public abstract void Parse(List<Request.PlayerStatsOnDate> list, Data.GameCenterResult gameCenter, Data.BoxScoreResult boxScore, string summaryReport);

        protected Request.PlayerStatsOnDate GetPlayerStat(List<Request.PlayerStatsOnDate> list, string dateId, int playerId)
        {
            var item = list.SingleOrDefault(x => (x.GameDateId == dateId) && (x.Player.ExternalId == playerId.ToString()));
            if (item == null)
            {
                var player = new Request.Player { ExternalId = playerId.ToString() };
                item = new Request.PlayerStatsOnDate
                {
                    GameDateId = dateId,
                    Player = player
                };
                list.Add(item);
            }
            return item;
        }


        protected Request.PlayerScore GetScoreItem(Request.PlayerStatsOnDate playerInfo, Request.ScoringType type)
        {
            var item = playerInfo.PlayerScores.SingleOrDefault(x => x.ScoringType.Name == type.Name);
            if (item == null)
            {
                item = new Request.PlayerScore
                {
                    ScoringType = type,
                    Total = 0
                };
                playerInfo.PlayerScores.Add(item);
            }
            return item;
        }

        protected IEnumerable<BoxScoreResult.PlayerStats> GetAllPlayerStats(BoxScoreResult boxScoreResult)
        {
            var gameStats = boxScoreResult.boxscore.playerByGameStats;
            var playerStatsList = new List<BoxScoreResult.PlayerStats>();
            playerStatsList.AddRange(gameStats.awayTeam.goalies);
            playerStatsList.AddRange(gameStats.awayTeam.defense);
            playerStatsList.AddRange(gameStats.awayTeam.forwards);
            playerStatsList.AddRange(gameStats.homeTeam.goalies);
            playerStatsList.AddRange(gameStats.homeTeam.defense);
            playerStatsList.AddRange(gameStats.homeTeam.forwards);

            return playerStatsList;
        }

        protected int? GetPlayerId(string name, string teamAbbrev, BoxScoreResult boxScoreResult)
        {
            BoxScoreResult.TeamPlayers teamPlayers;
            if (boxScoreResult.awayTeam.abbrev == teamAbbrev)
            {
                teamPlayers = boxScoreResult.boxscore.playerByGameStats.awayTeam;
            }
            else if (boxScoreResult.homeTeam.abbrev == teamAbbrev)
            {
                teamPlayers = boxScoreResult.boxscore.playerByGameStats.homeTeam;
            }
            else
            {
                return null;
            }

            // incoming
            //Joel Eriksson Ek

            // expected
            //J. Eriksson Ek
            var split = name.Split(' ');
            var firstName = split[0][0];
            var lastName = split[split.Length - 1];
            var playerMatches = teamPlayers.defense.Union(teamPlayers.forwards).Union(teamPlayers.goalies)
                .Where(x => x.name._default.StartsWith(firstName.ToString(), StringComparison.CurrentCultureIgnoreCase) && x.name._default.EndsWith(lastName, StringComparison.CurrentCultureIgnoreCase));
            // toodo warn on more than one match
            if (!playerMatches.Any())
            {
                return null;
            }
            return playerMatches.First().playerId;
        }
    }
}
