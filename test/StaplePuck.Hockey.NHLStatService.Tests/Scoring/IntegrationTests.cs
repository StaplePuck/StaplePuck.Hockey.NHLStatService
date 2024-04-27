using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StaplePuck.Hockey.NHLStatService.Scoring;

namespace StaplePuck.Hockey.NHLStatService.Tests.Scoring
{
    public class IntegrationTests
    {
        // fighting/ shootout https://api-web.nhle.com/v1/gamecenter/2023020964/boxscore
        // short handed/shut out/fighting https://www.nhl.com/gamecenter/pit-vs-wsh/2024/03/07/2023020998/boxscore

        private readonly StatsProvider _statsProvider;

        //20240316
        // need shutout. and series clinching goal
        //2023-05-29 shutout and series clinching goal
        public IntegrationTests()
        {
            var settings = new Settings
            {
                ApiUrlRoot = "https://api-web.nhle.com",
                StatsUrlRoot = "https://statsapi.web.nhl.com"
            };
            var loggerMock = new Mock<ILogger<StatsProvider>>();
            var parsers = new List<IScoringParser>();
            parsers.Add(new AssistParser());
            parsers.Add(new FightingParser());
            parsers.Add(new GoalDecisionParser());
            parsers.Add(new GoalParser());
            parsers.Add(new OvertimeGoalParser());
            parsers.Add(new SaveParser());
            parsers.Add(new ShootoutGoals());
            parsers.Add(new ShorthandedGoalParser());
            parsers.Add(new ShutoutParser());
            parsers.Add(new ThreeStarParser());
            parsers.Add(new WinParser());
            _statsProvider = new StatsProvider(Options.Create(settings), parsers, loggerMock.Object);
        }


        [Fact]
        public async Task TestRegularSeasonGames()
        {
            // arrange
            var dateId = "2024-03-16";

            // act
            var result = await _statsProvider.GetScoresForDateAsync(dateId, false);

            // assert
            Assert.NotNull(result);
            
            Assert.All(result, x=> { Assert.Equal(dateId, x.GameDateId); });

            // Assits
            var assistPlayer = result.Single(x => x.Player.ExternalId == "8480839");
            Assert.Collection(assistPlayer.PlayerScores, x =>
            {
                Assert.Equal("Assist", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // Fighting
            var fightingPlayer = result.Single(x => x.Player.ExternalId == "8477073");
            Assert.Collection(fightingPlayer.PlayerScores, x =>
            {
                Assert.Equal("Fighting Major", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // GWG
            var gameWinningGoal1 = result.Single(x => x.Player.ExternalId == "8474141");
            Assert.Collection(gameWinningGoal1.PlayerScores, x =>
            {
                Assert.Equal("Game Winning Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, 
            x=> {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("1st Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // GWG home, otg, goal
            var gameWinningGoal2 = result.Single(x => x.Player.ExternalId == "8480801");
            Assert.Collection(gameWinningGoal2.PlayerScores, x =>
            {
                Assert.Equal("Game Winning Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(3, x.Total);
            },
            x => {
                Assert.Equal("Overtime Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("1st Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // saves, win home
            var winHome = result.Single(x => x.Player.ExternalId == "8473503");
            Assert.Collection(winHome.PlayerScores, x =>
            {
                Assert.Equal("Save", x.ScoringType.Name);
                Assert.Equal(25, x.Total);
            },
            x => {
                Assert.Equal("Win", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // win Away
            var winAway = result.Single(x => x.Player.ExternalId == "8476914");
            Assert.Collection(winAway.PlayerScores, x =>
            {
                Assert.Equal("Save", x.ScoringType.Name);
                Assert.Equal(30, x.Total);
            },
            x => {
                Assert.Equal("Win", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // shootout goal
            var shootout = result.Single(x => x.Player.ExternalId == "8477404");
            Assert.Collection(shootout.PlayerScores,x =>
            {
                Assert.Equal("Assist", x.ScoringType.Name);
                Assert.Equal(2, x.Total);
            }, x =>
            {
                Assert.Equal("Shootout Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("1st Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // short handed goal
            var shortHanded = result.Single(x => x.Player.ExternalId == "8479314");
            Assert.Collection(shortHanded.PlayerScores, x =>
            {
                Assert.Equal("Assist", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Shorthanded Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("3rd Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // 2nd star
            var secondStar = result.Single(x => x.Player.ExternalId == "8478432");
            Assert.Collection(secondStar.PlayerScores, x =>
            {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total); Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("2nd Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // 3rd star
            var thirdStar = result.Single(x => x.Player.ExternalId == "8478466");
            Assert.Collection(thirdStar.PlayerScores, x =>
            {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            },
            x => {
                Assert.Equal("3rd Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
        }

        [Fact]
        public async Task TestPlayoffGames()
        {
            // arrange
            var dateId = "2023-05-29";

            // act
            var result = await _statsProvider.GetScoresForDateAsync(dateId, true);

            // assert
            Assert.NotNull(result);

            Assert.All(result, x => { Assert.Equal(dateId, x.GameDateId); });

            // series clinching goal
            var scgPlayer = result.Single(x => x.Player.ExternalId == "8477478");
            Assert.Collection(scgPlayer.PlayerScores, x =>
            {
                Assert.Equal("Assist", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Game Winning Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Series Clinching Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("3rd Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // shutouts
            var shutout = result.Single(x => x.Player.ExternalId == "8478499");
            Assert.Collection(shutout.PlayerScores, x =>
            {
                Assert.Equal("Save", x.ScoringType.Name);
                Assert.Equal(23, x.Total);
            }, x =>
            {
                Assert.Equal("Shutout", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("1st Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Win", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
        }

        [Fact]
        public async Task TestPlayoffGamesNonClinching()
        {
            // arrange
            var dateId = "2024-04-22";

            // act
            var result = await _statsProvider.GetScoresForDateAsync(dateId, true);

            // assert
            Assert.NotNull(result);

            Assert.All(result, x => { Assert.Equal(dateId, x.GameDateId); });

            // non series clinching goal
            var nonScgPlayer = result.Single(x => x.Player.ExternalId == "8475188");
            Assert.Collection(nonScgPlayer.PlayerScores, x =>
            {
                Assert.Equal("Game Winning Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Goal", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("1st Star", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            });
            // goalie assist
            var goalieAssit = result.Single(x => x.Player.ExternalId == "8479979");
            Assert.Collection(goalieAssit.PlayerScores, x =>
            {
                Assert.Equal("Assist", x.ScoringType.Name);
                Assert.Equal(1, x.Total);
            }, x =>
            {
                Assert.Equal("Save", x.ScoringType.Name);
                Assert.Equal(11, x.Total);
            });
        }
    }
}
