using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using StaplePuck.Core.Auth;
using StaplePuck.Core.Stats;
using StaplePuck.Core.Client;

namespace StaplePuck.Hockey.NHLStatService
{
    public class Updater
    {
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
            var teamStates = provider.GetTeamsStatesAsync(request.SeasonId).Result;


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
                TeamsStateOnDate = teamStates
            };
            gameDate.GameDateSeasons.Add(gds);
            //gameDate.GameDateSeasons = null;
            //gameDate.PlayersStatsOnDate = null;
            //gameDate.TeamsStateOnDate = null;

            var client = serviceProvider.GetService<IStaplePuckClient>();
            var result = client.UpdateAsync("updateGameDateStats", gameDate).Result;
        }
    }
}