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

namespace StaplePuck.Hockey.NHLStatService
{
    public class Updater
    {
        private readonly Settings _settings;
        private readonly StatsProvider _statsProvider;
        private readonly IStaplePuckClient _client;

        public Updater(IOptions<Settings> options, StatsProvider statsProvider, IStaplePuckClient client)
        {
            _settings = options.Value;
            _statsProvider = statsProvider;
            _client = client;
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

            var provider = serviceProvider.GetService<StatsProvider>();
            var playerScores = provider.GetScoresForDateAsync(request.GameDateId).Result;
            //var teamStates = provider.GetTeamsStatesAsync(request.SeasonId).Result;


            var season = new Season
            {
                ExternalId = request.SeasonId
            };
            var gds = new GameDateSeason
            {
                GameDateId = request.GameDateId,
                Season = season
            };

            var gameDate = new GameDate
            {
                Id = request.GameDateId,
                PlayersStatsOnDate = playerScores,
            };
            gameDate.GameDateSeasons.Add(gds);
            //gameDate.GameDateSeasons = null;
            //gameDate.PlayersStatsOnDate = null;
            //gameDate.TeamsStateOnDate = null;

            var client = serviceProvider.GetService<IStaplePuckClient>();
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

            var serviceProvider = new ServiceCollection()
                .AddOptions()
                .Configure<Settings>(configuration.GetSection("Settings"))
                .AddSingleton<StatsProvider>()
                .AddAuth0Client(configuration)
                .AddStaplePuckClient(configuration)
                .AddSingleton<Updater>()
                .BuildServiceProvider();

            return serviceProvider.GetService<Updater>();
        }

        public void Update()
        {
            bool done = false;
            var previousDateId = string.Empty;

            while (!done)
            {
                try
                {
                    var gameDateId = DateExtensions.TodaysDateId();

                    Console.Out.WriteLine($"Updating date: {gameDateId}");
                    var playerScores = _statsProvider.GetScoresForDateAsync(gameDateId).Result;

                    var season = new Season
                    {
                        ExternalId = _settings.SeasonId
                    };
                    var gds = new GameDateSeason
                    {
                        GameDateId = gameDateId,
                        Season = season
                    };

                    var gameDate = new GameDate
                    {
                        Id = gameDateId,
                        PlayersStatsOnDate = playerScores
                    };

                    if (gameDateId != previousDateId)
                    {
                        Console.Out.WriteLine("Getting team states");
                        var teamStates = _statsProvider.GetTeamsStatesAsync(_settings.SeasonId).Result;
                        var teamResult = _client.UpdateAsync("updateTeamStates", teamStates, "teamStates", "[TeamStateForSeasonInput]").Result;
                        if (teamResult == null)
                        {
                            Console.Error.WriteLine("Null result");
                        }
                        else if (!teamResult.Success)
                        {
                            Console.Error.WriteLine($"Failed to update. Message {teamResult.Message}");
                        }
                        Console.Out.WriteLine("Done updating date");
                    }

                    gameDate.GameDateSeasons.Add(gds);

                    Console.Out.WriteLine("Updating team date");
                    var result = _client.UpdateAsync("updateGameDateStats", gameDate).Result;
                    if (result == null)
                    {
                        Console.Error.WriteLine("Null result");
                    }
                    else if (!result.Success)
                    {
                        Console.Error.WriteLine($"Failed to update. Message {result.Message}");
                    }
                    Console.Out.WriteLine("Done updating date");
                    previousDateId = gameDateId;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Update failed. {e.Message}. {e.StackTrace}");
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

        public void UpdateDateRange(DateTime startDate, DateTime endDate)
        {
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var gameDateId = currentDate.ToGameDateId();

                Console.Out.WriteLine($"Updating date: {gameDateId}");
                try
                { 
                    var playerScores = _statsProvider.GetScoresForDateAsync(gameDateId).Result;

                    var season = new Season
                    {
                        ExternalId = _settings.SeasonId
                    };
                    var gds = new GameDateSeason
                    {
                        GameDateId = gameDateId,
                        Season = season
                    };

                    var gameDate = new GameDate
                    {
                        Id = gameDateId,
                        PlayersStatsOnDate = playerScores
                    };

                    gameDate.GameDateSeasons.Add(gds);
                    
                    Console.Out.WriteLine("Updating team date");
                    var result = _client.UpdateAsync("updateGameDateStats", gameDate).Result;
                    if (result == null)
                    {
                        Console.Error.WriteLine("Null result");
                    }
                    else if (!result.Success)
                    {
                        Console.Error.WriteLine($"Failed to update. Message {result.Message}");
                    }
                    Console.Out.WriteLine("Done updating date");
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine($"Update failed. {e.Message}. {e.StackTrace}");
                }

                currentDate = currentDate.AddDays(1);
            }
        }
    }

}