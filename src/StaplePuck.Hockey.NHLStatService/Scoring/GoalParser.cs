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

            foreach (var player in this.GetAllPlayerStats(boxScore).Where(x => x.goals != 0))
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, player.playerId);
                var saves = this.GetScoreItem(data, goalType);
                saves.Total = player.goals;
            }
        }
    }
}
