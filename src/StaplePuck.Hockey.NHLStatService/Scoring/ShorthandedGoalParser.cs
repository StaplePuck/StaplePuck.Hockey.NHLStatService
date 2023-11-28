using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class ShorthandedGoalParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var shorthandedType = new Request.ScoringType { Name = "Shorthanded Goal" };

            foreach (var player in this.GetAllPlayerStats(boxScore).Where(x => x.shorthandedGoals != 0))
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, player.playerId);
                var saves = this.GetScoreItem(data, shorthandedType);
                saves.Total = player.shorthandedGoals;
            }
        }
    }
}
