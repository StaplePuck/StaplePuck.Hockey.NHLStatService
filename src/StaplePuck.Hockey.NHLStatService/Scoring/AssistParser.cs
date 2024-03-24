namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class AssistParser : ScoringParser
    {
        public override void Parse(List<Request.PlayerStatsOnDate> list, Data.GameCenterResult gameCenter, Data.BoxScoreResult boxScore, string summaryReport)
        {
            var assistType = new Request.ScoringType { Name = "Assist" };

            foreach (var player in this.GetAllPlayerStats(boxScore).Where(x => x.assists != 0))
            {
                var data = this.GetPlayerStat(list, gameCenter.gameDate, player.playerId);
                var saves = this.GetScoreItem(data, assistType);
                saves.Total = player.assists;
            }
        }
    }
}
