using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class GoalParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var goalType = new Request.ScoringType { Name = "Goal" };

            foreach (var goal in gameCenter.summary.scoring.SelectMany(x => x.goals).Where(x => x.situationCode != "0101"))
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, goal.playerId);
                var goals = this.GetScoreItem(data, goalType);
                goals.Total++;
            }
        }
    }
}
