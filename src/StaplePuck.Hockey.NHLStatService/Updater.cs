using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StaplePuck.Core;
using StaplePuck.Core.Auth;
using StaplePuck.Core.Stats;
using StaplePuck.Core.Client;
using Microsoft.Extensions.Options;
using StaplePuck.Hockey.NHLStatService.Data;
using StaplePuck.Hockey.NHLStatService.Request;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StaplePuck.Hockey.NHLStatService
{
    public class Updater
    {
        private readonly Settings _settings;
        private readonly StatsProvider _statsProvider;
        private readonly IStaplePuckClient _client;
        private readonly ILogger _logger;

        public Updater(IOptions<Settings> options, StatsProvider statsProvider, IStaplePuckClient client, ILogger<Updater> logger)
        {
            _settings = options.Value;
            _statsProvider = statsProvider;
            _client = client;
            _logger = logger;
        }

        public static void UpdateDate(DateRequest request)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<Settings>(configuration.GetSection("Settings"))
                .AddSingleton<StatsProvider>()
                .AddAuth0Client(configuration)
                .AddStaplePuckClient(configuration)
                .BuildServiceProvider();

            var provider = serviceProvider.GetRequiredService<StatsProvider>();
            var playerScores = provider.GetScoresForDateAsync(request.GameDateId, request.IsPlayoffs).Result;
            //var teamStates = provider.GetTeamsStatesAsync(request.SeasonId).Result;

            if (playerScores == null)
            {
                return;
            }

            var season = new Request.Season
            {
                ExternalId = request.SeasonId
            };
            var gds = new Request.GameDateSeason
            {
                GameDateId = request.GameDateId,
                Season = season
            };

            var gameDate = new Request.GameDate
            {
                Id = request.GameDateId,
                PlayersStatsOnDate = playerScores,
            };
            gameDate.GameDateSeasons.Add(gds);
            //gameDate.GameDateSeasons = null;
            //gameDate.PlayersStatsOnDate = null;
            //gameDate.TeamsStateOnDate = null;

            var client = serviceProvider.GetRequiredService<IStaplePuckClient>();
            var result = client.UpdateAsync("updateGameDateStats", gameDate).Result;
            if (!result.Success)
            {
                Console.Error.WriteLine($"Failed to update stats. {result.Message}");
            }
        }

        public static Updater Init()
        {
            var builder = new ConfigurationBuilder()
                    .AddEnvironmentVariables();
            var configuration = builder.Build();

            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddOptions()
                        .Configure<Settings>(configuration.GetSection("Settings"))
                        .AddSingleton<StatsProvider>()
                        .AddAuth0Client(configuration)
                        .AddStaplePuckClient(configuration)
                        .AddSingleton<Updater>();
                })
                .AddNLog()
                .Build();


            return host.Services.GetRequiredService<Updater>();
        }

        public async Task UpdateRequest(DateRequest request)
        {
            try
            {
                var gameDateId = request.GameDateId;
                if (string.IsNullOrEmpty(gameDateId))
                {
                    gameDateId = DateTime.Now.ToGameDateId();
                }

                _logger.LogInformation($"Updating date: {gameDateId}");
                var playerScores = _statsProvider.GetScoresForDateAsync(gameDateId, request.IsPlayoffs).Result;
                if (playerScores == null) 
                { 
                    return; 
                }

                var season = new Request.Season
                {
                    ExternalId = request.SeasonId,
                    IsPlayoffs = request.IsPlayoffs
                };
                var gds = new Request.GameDateSeason
                {
                    GameDateId = gameDateId,
                    Season = season
                };

                var gameDate = new Request.GameDate
                {
                    Id = gameDateId,
                    PlayersStatsOnDate = playerScores
                };

                if (request.GetTeamStates)
                {
                    _logger.LogInformation("Getting team states");
                    var teamStates = _statsProvider.GetTeamsStatesAsync(request.SeasonId, request.IsPlayoffs).Result;
                    var teamResult = _client.UpdateAsync("updateTeamStates", teamStates, "teamStates", "[TeamStateForSeasonInput]").Result;
                    if (teamResult == null)
                    {
                        _logger.LogError("Null result");
                    }
                    else if (!teamResult.Success)
                    {
                        _logger.LogError($"Failed to update. Message {teamResult.Message}");
                    }
                    _logger.LogInformation("Done updating date");
                }

                gameDate.GameDateSeasons.Add(gds);

                _logger.LogInformation("Updating team date");
                var result = await _client.UpdateAsync("updateGameDateStats", gameDate);
                if (result == null)
                {
                    _logger.LogError("Null result");
                }
                else if (!result.Success)
                {
                    _logger.LogError($"Failed to update. Message {result.Message}");
                }
                _logger.LogInformation("Done updating date");
            }
            catch (Exception e)
            {
                _logger.LogError($"Update failed. {e.Message}. {e.StackTrace}");
            }
        }

        public void Update()
        {
            bool done = false;
            var previousDateId = string.Empty;

            while (!done)
            {
                try
                {
                    var gameDateId = StaplePuck.Core.DateExtensions.TodaysDateId();

                    _logger.LogInformation($"Updating date: {gameDateId}");
                    var playerScores = _statsProvider.GetScoresForDateAsync(gameDateId, true).Result;
                    if (playerScores == null)
                    {
                        continue;
                    }

                    var season = new Request.Season
                    {
                        ExternalId = _settings.SeasonId
                    };
                    var gds = new Request.GameDateSeason
                    {
                        GameDateId = gameDateId,
                        Season = season
                    };

                    var gameDate = new Request.GameDate
                    {
                        Id = gameDateId,
                        PlayersStatsOnDate = playerScores
                    };

                    if (gameDateId != previousDateId)
                    {
                        _logger.LogInformation("Getting team states");
                        var teamStates = _statsProvider.GetTeamsStatesAsync(_settings.SeasonId, true).Result;
                        var teamResult = _client.UpdateAsync("updateTeamStates", teamStates, "teamStates", "[TeamStateForSeasonInput]").Result;
                        if (teamResult == null)
                        {
                            _logger.LogError("Null result");
                        }
                        else if (!teamResult.Success)
                        {
                            _logger.LogError($"Failed to update. Message {teamResult.Message}");
                        }
                        _logger.LogInformation("Done updating date");
                    }

                    gameDate.GameDateSeasons.Add(gds);

                    _logger.LogInformation("Updating team date");
                    var result = _client.UpdateAsync("updateGameDateStats", gameDate).Result;
                    if (result == null)
                    {
                        _logger.LogError("Null result");
                    }
                    else if (!result.Success)
                    {
                        _logger.LogError($"Failed to update. Message {result.Message}");
                    }
                    _logger.LogInformation("Done updating date");
                    previousDateId = gameDateId;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Update failed. {e.Message}. {e.StackTrace}");
                }
                if (!_settings.Continuous)
                {
                    done = true;
                }
                else
                {
                    Task.Delay(_settings.Delay).Wait();
                }
            }
        }

        public void UpdateDateRange(DateTime startDate, DateTime endDate, bool isPlayoffs)
        {
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var gameDateId = currentDate.ToGameDateId();

                _logger.LogInformation($"Updating date: {gameDateId}");
                try
                { 
                    var playerScores = _statsProvider.GetScoresForDateAsync(gameDateId, isPlayoffs).Result;
                    if (playerScores == null) 
                    {
                        continue;
                    }

                    var season = new Request.Season
                    {
                        ExternalId = _settings.SeasonId
                    };
                    var gds = new Request.GameDateSeason
                    {
                        GameDateId = gameDateId,
                        Season = season
                    };

                    var gameDate = new Request.GameDate
                    {
                        Id = gameDateId,
                        PlayersStatsOnDate = playerScores
                    };

                    gameDate.GameDateSeasons.Add(gds);
                    
                    _logger.LogInformation("Updating team date");
                    var result = _client.UpdateAsync("updateGameDateStats", gameDate).Result;
                    if (result == null)
                    {
                        _logger.LogError("Null result");
                    }
                    else if (!result.Success)
                    {
                        _logger.LogError($"Failed to update. Message {result.Message}");
                    }
                    _logger.LogInformation("Done updating date");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Update failed. {e.Message}. {e.StackTrace}");
                }

                currentDate = currentDate.AddDays(1);
            }
        }
    }
}