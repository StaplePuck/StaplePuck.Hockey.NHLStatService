using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class GoalDecisionParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var gameWinningType = new ScoringType { Name = "Game Winning Goal" };
            var seriesClinchingType = new ScoringType { Name = "Series Clinching Goal" };

            var gwgNumber = -1;
            var winningTeamAbbrev = string.Empty;
            if (gameCenter.gameState != "OFF")
            {
                return;
            }

            // Determine GWG and Winner
            var awayScore = boxScore.awayTeam.score;
            var homeScore = boxScore.homeTeam.score;
            int gwgPlayerId = 0;
            if (awayScore < homeScore)
            {
                gwgNumber = awayScore + 1;

                var gwg = gameCenter.summary.scoring.SelectMany(x => x.goals).FirstOrDefault(x => x.teamAbbrev._default == boxScore.homeTeam.abbrev && x.homeScore == gwgNumber);
                if (gwg != null)
                {
                    gwgPlayerId = gwg.playerId;
                }
            }
            else
            {
                gwgNumber = homeScore + 1;

                var gwg = gameCenter.summary.scoring.SelectMany(x => x.goals).FirstOrDefault(x => x.teamAbbrev._default == boxScore.awayTeam.abbrev && x.awayScore == gwgNumber);
                if (gwg != null)
                {
                    gwgPlayerId = gwg.playerId;
                }
            }

            if (gwgPlayerId != 0)
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, gwgPlayerId);

                var stat = this.GetScoreItem(data, gameWinningType);
                stat.Total = 1;

                // TODO: SCG.. don't have enough data right now
                //gameCenter.summary.seasonSeries
            }
        }
    }
}
