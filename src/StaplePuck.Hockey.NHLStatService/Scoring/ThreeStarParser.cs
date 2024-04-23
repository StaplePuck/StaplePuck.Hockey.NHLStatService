using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class ThreeStarParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var firstStarType = new ScoringType { Name = "1st Star" };
            var secondStarType = new ScoringType { Name = "2nd Star" };
            var thirdStarType = new ScoringType { Name = "3rd Star" };

            var firstStar = gameCenter.summary.threeStars.FirstOrDefault(x => x.star == 1);
            if (firstStar != null)
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, firstStar.playerId);

                var stat = this.GetScoreItem(data, firstStarType);
                stat.Total++;
            }
            var secondStar = gameCenter.summary.threeStars.FirstOrDefault(x => x.star == 2);
            if (secondStar != null)
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, secondStar.playerId);

                var stat = this.GetScoreItem(data, secondStarType);
                stat.Total++;
            }
            var thirdStar = gameCenter.summary.threeStars.FirstOrDefault(x => x.star == 3);
            if (thirdStar != null)
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, thirdStar.playerId);

                var stat = this.GetScoreItem(data, thirdStarType);
                stat.Total++;
            }
        }
    }
}
