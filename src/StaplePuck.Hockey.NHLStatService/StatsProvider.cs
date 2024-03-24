using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StaplePuck.Core;
using StaplePuck.Hockey.NHLStatService.Scoring;

namespace StaplePuck.Hockey.NHLStatService
{
    public class StatsProvider
    {
        private readonly Settings _settings;
        private readonly IEnumerable<IScoringParser> _parsers;
        private readonly HttpClient _client = new HttpClient();
        private readonly ILogger _logger;

        public StatsProvider(IOptions<Settings> options, IEnumerable<IScoringParser> scoringParsers, ILogger<StatsProvider> logger)
        {
            _settings = options.Value;
            _parsers = scoringParsers;
            _logger = logger;
        }

        public async Task<List<Request.PlayerStatsOnDate>?> GetScoresForDateAsync(string dateId, bool isPlayoffs)
        {
            // get games
            var url = $"{_settings.ApiUrlRoot}/v1/score/{dateId}";
            var dateResult = await _client.GetAsync(url);

            if (!dateResult.IsSuccessStatusCode)
            {
                return null;
            }

            var winType = new Request.ScoringType { Name = "Win" };
            var shootoutGoalType = new Request.ScoringType { Name = "Shootout Goal" };

            var content = await dateResult.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Data.ScoreDateResult>(content);
            if (value == null)
            {
                return null;
            }
            var list = new List<Request.PlayerStatsOnDate>();

            foreach (var game in value.games)
            {
                // 2 = regular   3== playoffs
                if (!((isPlayoffs && game.gameType == 3) || (!isPlayoffs && game.gameType == 2)))
                {
                    continue;
                }
                if (game.gameState == "FUT" || game.gameState == "PRE")
                {
                    continue;
                }
                var landingUrl = $"{_settings.ApiUrlRoot}/v1/gamecenter/{game.id}/landing";
                var liveTask = _client.GetAsync(landingUrl);

                var boxScoreUrl = $"{_settings.ApiUrlRoot}/v1/gamecenter/{game.id}/boxscore";
                var boxScoreTask = _client.GetAsync(boxScoreUrl);

                await Task.WhenAll(liveTask, boxScoreTask);

                if (!liveTask.Result.IsSuccessStatusCode || !boxScoreTask.Result.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to get game data {game.id}");
                    continue;
                }

                var liveContentTask = liveTask.Result.Content.ReadAsStringAsync();
                var boxScoreContentTask = boxScoreTask.Result.Content.ReadAsStringAsync();
                await Task.WhenAll(liveContentTask, boxScoreContentTask);

                var liveData = JsonConvert.DeserializeObject<Data.GameCenterResult>(liveContentTask.Result);
                var boxScoreData = JsonConvert.DeserializeObject<Data.BoxScoreResult>(boxScoreContentTask.Result);

                if (boxScoreData == null)
                {
                    _logger.LogWarning("No content for boxscore data");
                    continue;
                }
                if (liveData == null)
                {
                    _logger.LogWarning("No content for live data");
                    continue;
                }

                var gameSummary = string.Empty;
                if (!string.IsNullOrEmpty(boxScoreData.summary.gameReports.gameSummary))
                {
                    gameSummary = await _client.GetStringAsync(boxScoreData.summary.gameReports.gameSummary);
                }

                foreach (var parser in _parsers)
                {
                    parser.Parse(list, liveData, boxScoreData, gameSummary);
                }
            }

            return list;
        }

        private Request.PlayerStatsOnDate GetPlayerStat(List<Request.PlayerStatsOnDate> list, string dateId, int playerId)
        {
            var item = list.SingleOrDefault(x => (x.GameDateId == dateId) && (x.Player.ExternalId == playerId.ToString()));
            if (item == null)
            {
                var player = new Request.Player { ExternalId = playerId.ToString() };
                item = new Request.PlayerStatsOnDate
                {
                    GameDateId = dateId,
                    Player = player
                };
                list.Add(item);
            }
            return item;
        }


        private Request.PlayerScore GetScoreItem(Request.PlayerStatsOnDate playerInfo, Request.ScoringType type)
        {
            var item = playerInfo.PlayerScores.SingleOrDefault(x => x.ScoringType.Name == type.Name);
            if (item == null)
            {
                item = new Request.PlayerScore
                {
                    ScoringType = type,
                    Total = 0
                };
                playerInfo.PlayerScores.Add(item);
            }
            return item;
        }

        public async Task<List<Request.TeamStateForSeason>?> GetTeamsStatesAsync(string seasonId, bool isPlayoffs)
        {
            if (isPlayoffs)
            {
                var url = string.Format("{0}/api/v1/tournaments/playoffs?expand=round.series&season={1}&site=en_nhl", _settings.StatsUrlRoot, seasonId);
                var dateResult = await _client.GetAsync(url);

                if (!dateResult.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await dateResult.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Data.TournamentResult>(content);
                if (value == null)
                {
                    return null;
                }
                var list = new List<Request.TeamStateForSeason>();
                foreach (var item in GetTeamStates(value))
                {
                    var team = new Request.Team { ExternalId = item.Key };
                    var season = new Request.Season { ExternalId = seasonId, IsPlayoffs = isPlayoffs };
                    var teamState = new Request.TeamStateForSeason { Season = season, Team = team, GameState = item.Value };
                    list.Add(teamState);
                }
                return list;
            }
            else
            {
                var url = string.Format("{0}/api/v1/standings?hydrate=team(nextSchedule(team))&season={1}&site=en_nhl", _settings.StatsUrlRoot, seasonId);
                var dateResult = await _client.GetAsync(url);

                if (!dateResult.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await dateResult.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<Data.StandingsResult>(content);

                var list = new List<Request.TeamStateForSeason>();
                foreach (var item in value!.records!.SelectMany(x => x.teamRecords))
                {
                    var team = new Request.Team { ExternalId = item!.team!.id };
                    var season = new Request.Season { ExternalId = seasonId, IsPlayoffs = isPlayoffs };
                    var gameState = 0;
                    var date = item.team?.nextSchedule?.dates?.FirstOrDefault();
                    if (date?.date != null && date.date.Value.IsToday() && date.games.Any(x => x.gameType == "P"))
                    {
                        gameState = 1;
                    }
                    var teamState = new Request.TeamStateForSeason { Season = season, Team = team, GameState = gameState };
                    list.Add(teamState);
                }
                return list;
            }
        }

        public Dictionary<int, int> GetTeamStates(Data.TournamentResult result)
        {
            var teams = new Dictionary<int, int>();
            foreach (var round in result.rounds)
            {
                foreach (var series in round.series)
                {
                    if (series.matchupTeams != null && series.matchupTeams.Length == 2)
                    {
                        var teamA = series.matchupTeams[0];
                        var teamB = series.matchupTeams[1];
                        var gameTime = series!.currentGame!.seriesSummary!.gameTime;
                        UpdateTeam(teams, teamA, series.currentGame.seriesSummary, gameTime!.Value!);
                        UpdateTeam(teams, teamB, series.currentGame.seriesSummary, gameTime.Value);
                    }
                }
            }
            return teams;
        }

        private void UpdateTeam(Dictionary<int, int> teams, Data.Matchupteam matchup, Data.Seriessummary seriesSummary, DateTime gameTime)
        {
            var teamId = matchup!.team!.id;
            teams[teamId] = 0;

            if (seriesSummary.seriesStatus.Contains(" win "))
            {
                if (matchup!.seriesRecord!.losses > matchup.seriesRecord.wins)
                {
                    teams[teamId] = -1;
                }
            }

            if (gameTime.IsToday())
            {
                teams[teamId] = 1;
            }
        }
    }
}
