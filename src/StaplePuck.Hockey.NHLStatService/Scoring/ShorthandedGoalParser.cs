using StaplePuck.Hockey.NHLStatService.Data;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class ShorthandedGoalParser : ScoringParser
    {
        public override void Parse(List<Request.PlayerStatsOnDate> list, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var shorthandedType = new Request.ScoringType { Name = "Shorthanded Goal" };

            foreach (var goal in gameCenter.summary.scoring.SelectMany(x => x.goals).Where(x => x.strength == "sh"))
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, goal.playerId);
                var shortHandedGoal = this.GetScoreItem(data, shorthandedType);
                shortHandedGoal.Total++;
            }
        }
    }
}
