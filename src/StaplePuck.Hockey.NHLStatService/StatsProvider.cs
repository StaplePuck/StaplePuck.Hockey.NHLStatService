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

        public async Task<List<PlayerStatsOnDate>> GetScoresForDateAsync(string dateId)
        {
            //var playersResult = this.m_context.HockeyPlayers.ToListAsync();
            //https://statsapi.web.nhl.com/api/v1/schedule?startDate=2016-04-13&endDate=2016-04-13&expand=schedule.decisions,schedule.scoringplays&site=en_nhl&teamId=
            var url = string.Format("{0}/schedule?startDate={1}&endDate={1}&expand=schedule.decisions,schedule.scoringplays,schedule.game.seriesSummary&site=en_nhl&teamId=&gameType=P", _settings.StatsUrlRoot, dateId);
            var dateResult = await _client.GetAsync(url);

            if (!dateResult.IsSuccessStatusCode)
            {
                return null;
            }

            var goalType = new ScoringType { Name = "Goal" };
            var assistType = new ScoringType { Name = "Assist" };
            var winType = new ScoringType { Name = "Win" };
            var shutoutType = new ScoringType { Name = "Shutout" };
            var shorthandedType = new ScoringType { Name = "Shorthanded Goal" };
            var gameWinningType = new ScoringType { Name = "Game Winning Goal" };
            var seriesClinchingType = new ScoringType { Name = "Series Clinching Goal" };
            var overtimeGoalType = new ScoringType { Name = "Overtime Goal" };

            var content = await dateResult.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Data.DateResult>(content);
            var list = new List<PlayerStatsOnDate>();

            foreach (var date in value.dates)
            {
                foreach (var game in date.games)
                {
                    var gwgNumber = -1;
                    var winningTeam = string.Empty;
                    if (game.status.IsOver)
                    {
                        // Determine GWG and Winner
                        var awayScore = game.teams.away.score;
                        var homeScore = game.teams.home.score;
                        if (awayScore < homeScore)
                        {
                            gwgNumber = awayScore + 1;
                            winningTeam = game.teams.home.team.name;
                        }
                        else
                        {
                            gwgNumber = homeScore + 1;
                            winningTeam = game.teams.away.team.name;
                        }
                    }

                    // Scores
                    int position = 1;
                    foreach (var play in game.scoringPlays)
                    {
                        var strength = play.result.strength;
                        foreach (var player in play.players)
                        {
                            var data = this.GetPlayerStat(list, date.date, player.player.id);
                            if (player.playerType == "Assist")
                            {
                                var score = this.GetScoreItem(data, assistType);
                                score.Total++;
                            }
                            else if (player.playerType == "Scorer")
                            {
                                if (play.about.periodType == Data.PeriodType.OVERTIME && play.about.periodTime == "00:00")
                                {
                                }
                                else
                                {
                                    var goal = this.GetScoreItem(data, goalType);
                                    goal.Total++;
                                }

                                if (play.result.strength.code == Data.StrengthCode.SHG)
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

                                if (winningTeam.Equals(play.team.name) && position == gwgNumber && game.seriesSummary != null)
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
                        if (winningTeam.Equals(play.team.name))
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
                        if (game.teams.LosingScore == 0)
                        {
                            var shutout = this.GetScoreItem(data, shutoutType);
                            shutout.Total++;
                        }
                    }
                    if (game.decisions == null && game.status.IsOver)
                    {
                        Console.Out.WriteLine($"Warning game is over but no decisions. Date: {game.gameDate} {game.teams.away.team.name} at {game.teams.home.team.name}");
                    }
                }
            }

            return list;
        }

        private PlayerStatsOnDate GetPlayerStat(List<PlayerStatsOnDate> list, string dateId, int playerId)
        {
            var item = list.SingleOrDefault(x => (x.GameDateId == dateId) && (x.Player.ExternalId == playerId.ToString()));
            if (item == null)
            {
                var player = new Player { ExternalId = playerId.ToString() };
                item = new PlayerStatsOnDate
                {
                    GameDateId = dateId,
                    Player = player
                };
                list.Add(item);
            }
            return item;
        }


        private PlayerScore GetScoreItem(PlayerStatsOnDate playerInfo, ScoringType type)
        {
            var item = playerInfo.PlayerScores.SingleOrDefault(x => x.ScoringType.Name == type.Name);
            if (item == null)
            {
                item = new PlayerScore
                {
                    ScoringType = type,
                    Total = 0
                };
                playerInfo.PlayerScores.Add(item);
            }
            return item;
        }

        public async Task<List<TeamStateForSeason>> GetTeamsStatesAsync(string seasonId)
        {
            var url = string.Format("{0}/tournaments/playoffs?expand=round.series&season={1}&site=en_nhl", _settings.StatsUrlRoot, seasonId);
            var dateResult = await _client.GetAsync(url);

            if (!dateResult.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await dateResult.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<Data.TournamentResult>(content);

            var list = new List<TeamStateForSeason>();
            foreach (var item in GetTeamStates(value))
            {
                var team = new Team { ExternalId = item.Key };
                var season = new Season { ExternalId = seasonId };
                var teamState = new TeamStateForSeason { Season = season, Team = team, GameState = item.Value };
                list.Add(teamState);
            }
            return list;
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
                        var gameTime = series.currentGame.seriesSummary.gameTime;
                        UpdateTeam(teams, teamA, series.currentGame.seriesSummary, gameTime);
                        UpdateTeam(teams, teamB, series.currentGame.seriesSummary, gameTime);
                    }
                }
            }
            return teams;
        }

        private void UpdateTeam(Dictionary<int, int> teams, Data.Matchupteam matchup, Data.Seriessummary seriesSummary, DateTime gameTime)
        {
            var teamId = matchup.team.id;
            teams[teamId] = 0;

            if (seriesSummary.seriesStatus.Contains(" win "))
            {
                if (matchup.seriesRecord.losses > matchup.seriesRecord.wins)
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
