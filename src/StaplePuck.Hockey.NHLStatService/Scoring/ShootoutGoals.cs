using StaplePuck.Core.Stats;
using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class ShootoutGoals : ScoringParser
    {
        public override void Parse(List<Request.PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var shootoutGoalType = new Request.ScoringType { Name = "Shootout Goal" };

            foreach (var goal in gameCenter.summary.shootout.Where(x => x.result == "goal")) 
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, goal.playerId);
                var saves = this.GetScoreItem(data, shootoutGoalType);
                saves.Total++;
            }
        }
    }
}
