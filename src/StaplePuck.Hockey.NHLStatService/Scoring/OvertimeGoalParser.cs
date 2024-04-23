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
    public class OvertimeGoalParser : ScoringParser
    {
        public override void Parse(List<Request.PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var overtimeGoalType = new Request.ScoringType { Name = "Overtime Goal" };

            foreach (var goal in gameCenter.summary.scoring.Where(x => x.periodDescriptor.periodType == "OT").SelectMany(x => x.goals)) 
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, goal.playerId);
                var otg = this.GetScoreItem(data, overtimeGoalType);
                otg.Total = 1;
            }
        }
    }
}
