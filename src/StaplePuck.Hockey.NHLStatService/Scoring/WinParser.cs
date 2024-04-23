using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class WinParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var winType = new Request.ScoringType { Name = "Win" };
            if (gameCenter.gameState != "OFF")
            {
                return;
            }

            string teamAbbrev;
            var awayScore = boxScore.awayTeam.score;
            var homeScore = boxScore.homeTeam.score;
            if (awayScore > homeScore)
            {
                teamAbbrev = boxScore.awayTeam.abbrev;
            }
            else
            {
                teamAbbrev = boxScore.homeTeam.abbrev;
            }

            //>WOLL, JOSEPH (W) <
            var regex = new Regex(">(?<lastName>[A-Za-z\\.\\- ]+), (?<firstName>[A-Za-z0-9\\.\\- ]+) \\(W\\) </td>", RegexOptions.Multiline);
            var match = regex.Match(summaryReport);
            if (match.Success) 
            { 
                var playerName = $"{match.Groups["firstName"].Value} {match.Groups["lastName"].Value}";

                var playerId = GetPlayerId(playerName, teamAbbrev, boxScore);
                if (playerId == null)
                {
                    // todo log warning
                    return;
                }

                var data = this.GetPlayerStat(list, gameCenter.gameDate, playerId.Value);
                var wins = this.GetScoreItem(data, winType);
                wins.Total++;
            }
            else
            {
                // todo log error
            }
        }
    }
}
