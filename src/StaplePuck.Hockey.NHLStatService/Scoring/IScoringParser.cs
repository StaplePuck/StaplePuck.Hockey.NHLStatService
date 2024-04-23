using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public interface IScoringParser
    {
        void Parse(List<PlayerStatsOnDate> list, ScoreDateResult.Game game, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport);
    }
}