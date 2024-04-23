using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static StaplePuck.Hockey.NHLStatService.Data.LiveResult;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class SaveParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var saveType = new ScoringType { Name = "Save" };

            foreach (var goalie in boxScore.playerByGameStats.homeTeam.goalies
                   .Union(boxScore.playerByGameStats.awayTeam.goalies))
            {
                var shotSplit = goalie.saveShotsAgainst.Split("/");
                if (shotSplit.Length == 2 && shotSplit[0] != "0")
                {
                    var saveStat = int.Parse(shotSplit[0]);
                    var data = this.GetPlayerStat(list, gameCenter.gameDate, goalie.playerId);
                    var saves = this.GetScoreItem(data, saveType);
                    saves.Total = saveStat;
                }
            }
        }
    }
}
