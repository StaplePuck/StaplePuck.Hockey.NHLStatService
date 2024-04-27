using StaplePuck.Hockey.NHLStatService.Data;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class AssistParser : ScoringParser
    {
        public override void Parse(List<Request.PlayerStatsOnDate> list, ScoreDateResult.Game game, Data.GameCenterResult gameCenter, Data.BoxScoreResult boxScore, string summaryReport)
        {
            var assistType = new Request.ScoringType { Name = "Assist" };

            //foreach (var player in this.GetAllPlayerStats(boxScore).Where(x => x.assists != 0))
            //{
            //    var data = this.GetPlayerStat(list, gameCenter.gameDate, player.playerId);
            //    var assists = this.GetScoreItem(data, assistType);
            //    assists.Total = player.assists;
            //}

            foreach (var assist in gameCenter.summary.scoring.SelectMany(x => x.goals).SelectMany(x => x.assists))
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, assist.playerId);
                var assists = this.GetScoreItem(data, assistType);
                assists.Total++;
            }
        }
    }
}
