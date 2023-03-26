using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using StaplePuck.Core.Stats;
using StaplePuck.Core;
using System.Text.RegularExpressions;
using static StaplePuck.Hockey.NHLStatService.Data.LiveResult;
using StaplePuck.Hockey.NHLStatService.Request;

namespace StaplePuck.Hockey.NHLStatService
{
    public class StatsProvider
    {
        private readonly Settings _settings;
        private readonly HttpClient _client = new HttpClient();

        public StatsProvider(IOptions<Settings> options)
        {
            _settings = options.Value;
        }

        public async Task<List<Request.PlayerStatsOnDate>?> GetScoresForDateAsync(string dateId, bool isPlayoffs)
        {
            //var playersResult = this.m_context.HockeyPlayers.ToListAsync();
            //https://statsapi.web.nhl.com/api/v1/schedule?startDate=2016-04-13&endDate=2016-04-13&expand=schedule.decisions,schedule.scoringplays&site=en_nhl&teamId=
            var url = string.Format("{0}/api/v1/schedule?startDate={1}&endDate={1}&expand=schedule.decisions,schedule.scoringplays,schedule.game.seriesSummary&site=en_nhl", _settings.StatsUrlRoot, dateId);
            var dateResult = await _client.GetAsync(url);

            if (!dateResult.IsSuccessStatusCode)
            {
                return null;
            }

            var goalType = new Request.ScoringType { Name = "Goal" };
            var assistType = new Request.ScoringType { Name = "Assist" };
            var winType = new Request.ScoringType { Name = "Win" };
            var shutoutType = new Request.ScoringType { Name = "Shutout" };
            var shorthandedType = new Request.ScoringType { Name = "Shorthanded Goal" };
            var gameWinningType = new Request.ScoringType { Name = "Game Winning Goal" };
            var seriesClinchingType = new Request.ScoringType { Name = "Series Clinching Goal" };
            var overtimeGoalType = new Request.ScoringType { Name = "Overtime Goal" };
            var shootoutGoalType = new Request.ScoringType { Name = "Shootout Goal" };
            var fightingMajorType = new Request.ScoringType { Name = "Fighting Major" };
            var saveType = new Request.ScoringType { Name = "Save" };
            var firstStarType = new Request.ScoringType { Name = "1st Star" };
            var secondStarType = new Request.ScoringType { Name = "2nd Star" };
            var thirdStarType = new Request.ScoringType { Name = "3rd Star" };

            var content = await dateResult.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Data.DateResult>(content);
            if (value == null)
            {
                return null;
            }
            var list = new List<Request.PlayerStatsOnDate>();

            foreach (var date in value.dates)
            {
                foreach (var game in date.games)
                {
                    if (!((isPlayoffs && game.gameType == "P") || (!isPlayoffs && game.gameType == "R")))
                    {
                        continue;
                    }
                    var liveResult = await _client.GetAsync($"{_settings.StatsUrlRoot}{game.link}");

                    if (liveResult.IsSuccessStatusCode)
                    {
                        var liveContent = await liveResult.Content.ReadAsStringAsync();
                        var liveData = JsonConvert.DeserializeObject<Data.LiveResult>(liveContent);

                        // saves
                        foreach (var goalie in liveData!.liveData!.boxscore!.teams!.home!.players.Values.Where(x => x.stats?.goalieStats != null)
                            .Union(liveData!.liveData!.boxscore!.teams!.away!.players.Values.Where(x => x.stats?.goalieStats != null)))
                        {
                            if (goalie!.stats!.goalieStats!.saves > 0)
                            {
                                var data = this.GetPlayerStat(list, date.date, goalie!.person!.id!);
                                var saves = this.GetScoreItem(data, saveType);
                                saves.Total = goalie.stats.goalieStats.saves;
                            }
                        }

                        // penalties
                        foreach (var penality in liveData!.liveData!.plays!.allPlays.Where(
                            x => x.result?.eventTypeId == "PENALTY" && x.result?.secondaryType == "Fighting"))
                        {
                            foreach (var player in penality.players)
                            {
                                var data = this.GetPlayerStat(list, date.date, player!.player!.id!);
                                var fightings = this.GetScoreItem(data, fightingMajorType);
                                fightings.Total++;
                            }
                        }
                    }
                    else
                    {
                        //log warning
                    }

                    var gwgNumber = -1;
                    var winningTeam = string.Empty;
                    if (game?.status?.IsOver ?? false && game?.teams?.away!.team != null && game?.teams?.home!.team != null)
                    {
                        // Determine GWG and Winner
                        var awayScore = game!.teams!.away.score;
                        var homeScore = game.teams.home.score;
                        if (awayScore < homeScore)
                        {
                            gwgNumber = awayScore + 1;
                            winningTeam = game!.teams!.home!.team!.name;
                        }
                        else
                        {
                            gwgNumber = homeScore + 1;
                            winningTeam = game!.teams!.away!.team!.name;
                        }
                    }

                    // Scores
                    int position = 1;
                    foreach (var play in game!.scoringPlays!)
                    {
                        var strength = play!.result!.strength;
                        foreach (var player in play.players)
                        {
                            var data = this.GetPlayerStat(list, date.date, player!.player!.id);
                            if (player.playerType == "Assist")
                            {
                                var score = this.GetScoreItem(data, assistType);
                                score.Total++;
                            }
                            else if (player.playerType == "Scorer")
                            {
                                if (play!.about!.periodType == Data.PeriodType.OVERTIME && play.about.periodTime == "00:00")
                                {
                                    //shoot out goal
                                    var shootoutGoal = this.GetScoreItem(data, shootoutGoalType);
                                    shootoutGoal.Total++;
                                }
                                else
                                {
                                    var goal = this.GetScoreItem(data, goalType);
                                    goal.Total++;

                                    if (play!.result!.strength!.code == Data.StrengthCode.SHG)
                                    {
                                        var shorthanded = this.GetScoreItem(data, shorthandedType);
                                        shorthanded.Total++;
                                    }
                                    if (play.about.periodType == Data.PeriodType.OVERTIME && play.about.periodTime != "00:00")
                                    {
                                        //not a shoot out goal in over time
                                        var overtimeGoal = this.GetScoreItem(data, overtimeGoalType);
                                        overtimeGoal.Total++;
                                    }

                                    if (winningTeam.Equals(play!.team!.name) && position == gwgNumber && game.seriesSummary != null)
                                    {
                                        // Calculate if scg
                                        if (game.seriesSummary.IsOver)
                                        {
                                            var seriesClinch = this.GetScoreItem(data, seriesClinchingType);
                                            seriesClinch.Total++;
                                        }
                                        // GWG
                                        var gameWinning = this.GetScoreItem(data, gameWinningType);
                                        gameWinning.Total++;
                                    }
                                }
                            }
                        }
                        if (winningTeam.Equals(play!.team!.name))
                        {
                            position++;
                        }
                    }

                    // goalie
                    if (game?.decisions?.winner != null)
                    {
                        var goalieWinnerId = game.decisions.winner.id;
                        var data = this.GetPlayerStat(list, date.date, goalieWinnerId);

                        var win = this.GetScoreItem(data, winType);
                        win.Total++;
                        if (game!.teams!.LosingScore == 0)
                        {
                            var shutout = this.GetScoreItem(data, shutoutType);
                            shutout.Total++;
                        }
                    }
                    if (game?.decisions?.firstStar != null)
                    {
                        var playerId = game.decisions.firstStar.id;
                        var data = this.GetPlayerStat(list, date.date, playerId);

                        var firstStar = this.GetScoreItem(data, firstStarType);
                        firstStar.Total++;
                    }
                    if (game?.decisions?.secondStar != null)
                    {
                        var playerId = game.decisions.secondStar.id;
                        var data = this.GetPlayerStat(list, date.date, playerId);

                        var secondStar = this.GetScoreItem(data, secondStarType);
                        secondStar.Total++;
                    }
                    if (game?.decisions?.thirdStar != null)
                    {
                        var playerId = game.decisions.thirdStar.id;
                        var data = this.GetPlayerStat(list, date.date, playerId);

                        var thirdStar = this.GetScoreItem(data, thirdStarType);
                        thirdStar.Total++;
                    }
                    if (game!.decisions! == null && game!.status!.IsOver)
                    {
                        Console.Out.WriteLine($"Warning game is over but no decisions. Date: {game.gameDate} {game!.teams!.away!.team!.name} at {game!.teams!.home!.team!.name}");
                    }
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
