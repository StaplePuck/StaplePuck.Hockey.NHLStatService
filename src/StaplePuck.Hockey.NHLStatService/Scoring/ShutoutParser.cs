using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class ShutoutParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            if (gameCenter.gameState != "OFF")
            {
                return;
            }

            var shutoutType = new ScoringType { Name = "Shutout" };
            if (boxScore.awayTeam.score == 0)
            {
                foreach (var goalie in boxScore.boxscore.playerByGameStats.homeTeam.goalies)
                {
                    if (goalie.toi != "00:00")
                    {
                        var data = this.GetPlayerStat(list, gameCenter.gameDate, goalie.playerId);
                        var shutOut = this.GetScoreItem(data, shutoutType);
                        shutOut.Total = 1;
                    }
                }
            }

            if (boxScore.homeTeam.score == 0) 
            {
                foreach (var goalie in boxScore.boxscore.playerByGameStats.awayTeam.goalies)
                {
                    if (goalie.toi != "00:00")
                    {
                        var data = this.GetPlayerStat(list, gameCenter.gameDate, goalie.playerId);
                        var shutOut = this.GetScoreItem(data, shutoutType);
                        shutOut.Total = 1;
                    }
                }
            }
        }
    }
}
