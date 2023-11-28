using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;

namespace StaplePuck.Hockey.NHLStatService.Scoring
{
    public class FightingParser : ScoringParser
    {
        public override void Parse(List<PlayerStatsOnDate> list, GameCenterResult gameCenter, BoxScoreResult boxScore, string summaryReport)
        {
            var fightingMajorType = new Request.ScoringType { Name = "Fighting Major" };

            foreach (var penality in gameCenter.summary.penalties.SelectMany(x => x.penalties).Where(x => x.descKey == "fighting"))
            {
                var playerName = penality.committedByPlayer;

                var playerId = GetPlayerId(playerName, penality.teamAbbrev, boxScore);
                if (playerId == null)
                {
                    // todo log warning
                    continue;
                }

                var data = this.GetPlayerStat(list, gameCenter.gameDate, playerId.Value);
                var fightings = this.GetScoreItem(data, fightingMajorType);
                fightings.Total++;
            }
        }
    }
}
