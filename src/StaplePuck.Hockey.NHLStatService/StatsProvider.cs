using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StaplePuck.Core;
using StaplePuck.Hockey.NHLStatService.Data;
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
                    parser.Parse(list, game, liveData, boxScoreData, gameSummary);
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

        public async Task<List<Request.TeamStateForSeason>?> GetTeamsStatesAsync(string seasonId, string gameDate, bool isPlayoffs)
        {
            // if playoffs get bracket
            Dictionary<int, int> teams;

            // https://api-web.nhle.com/v1/playoff-bracket/2024
            if (isPlayoffs)
            {
                var playoffSeson = seasonId[^4..];
                var braketUrl = $"{_settings.ApiUrlRoot}/v1/playoff-bracket/{playoffSeson}";
                var bracketResult = await _client.GetAsync(braketUrl);

                if (!bracketResult.IsSuccessStatusCode)
                {
                    return null;
                }

                var bracketContent = await bracketResult.Content.ReadAsStringAsync();
                var bracketValue = JsonConvert.DeserializeObject<Data.BracketResult>(bracketContent);
                if (bracketValue == null)
                {
                    return null;
                }

                teams = GetTeamStates(bracketValue);
            }
            else
            {
                teams = new Dictionary<int, int>();
                // todo...
                // v1/standings/2024-04-20
                //var url = string.Format("{0}/api/v1/standings?hydrate=team(nextSchedule(team))&season={1}&site=en_nhl", _settings.StatsUrlRoot, seasonId);
                //var standingsResult = await _client.GetAsync(url);

                //if (!standingsResult.IsSuccessStatusCode)
                //{
                //    return null;
                //}

                //var standingsContent = await standingsResult.Content.ReadAsStringAsync();
                //var standingsValue = JsonConvert.DeserializeObject<Data.StandingsResult>(standingsContent);

                //foreach (var item in standingsValue!.standings)
                //{
                //    var team = new Request.Team { ExternalId = item. item!.team!.id };
                //    var season = new Request.Season { ExternalId = seasonId, IsPlayoffs = isPlayoffs };
                //    var gameState = 0;
                //    var date = item.team?.nextSchedule?.dates?.FirstOrDefault();
                //    if (date?.date != null && date.date.Value.IsToday() && date.games.Any(x => x.gameType == "P"))
                //    {
                //        gameState = 1;
                //    }
                //    var teamState = new Request.TeamStateForSeason { Season = season, Team = team, GameState = gameState };
                //    list.Add(teamState);
                //}
                //return list;
            }

            // get game date data and update the playing today data
            var url = $"{_settings.ApiUrlRoot}/v1/score/{gameDate}";
            var dateResult = await _client.GetAsync(url);

            if (!dateResult.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await dateResult.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Data.ScoreDateResult>(content);
            foreach (var item in value!.games)
            {
                UpdateTeam(teams, item.awayTeam.id, 1);
                UpdateTeam(teams, item.homeTeam.id, 1);
            }

            var list = new List<Request.TeamStateForSeason>();
            foreach (var item in teams)
            {
                var team = new Request.Team { ExternalId = item.Key };
                var season = new Request.Season { ExternalId = seasonId, IsPlayoffs = isPlayoffs };
                var teamState = new Request.TeamStateForSeason { Season = season, Team = team, GameState = item.Value };
                list.Add(teamState);
            }
            return list;
        }

        public Dictionary<int, int> GetTeamStates(BracketResult result)
        {
            var teams = new Dictionary<int, int>();
            foreach (var series in result.series)
            {
                if (series.winningTeamId > 0)
                {
                    UpdateTeam(teams, series.winningTeamId, 0);
                    UpdateTeam(teams, series.losingTeamId, -1);
                }
                else
                {
                    UpdateTeam(teams, series.topSeedTeam.id, 0);
                    UpdateTeam(teams, series.bottomSeedTeam.id, 0);
                }
            }
            return teams;
        }

        private void UpdateTeam(Dictionary<int, int> teams, int teamId, int teamState)
        {
            if (teamId == 0)
            {
                return;
            }
            if (teams.TryGetValue(teamId, out var existingState) && existingState > 0)
            {
                teams[teamId] = teamState;
            }
            else
            {
                teams[teamId] = teamState;
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
