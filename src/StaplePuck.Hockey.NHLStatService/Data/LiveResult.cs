using System;
using System.Collections.Generic;
using System.Text;
using static StaplePuck.Hockey.NHLStatService.Data.LiveResult;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    internal class LiveResult
    {
        public string copyright { get; set; }
        public int gamePk { get; set; }
        public string link { get; set; }
        public Metadata metaData { get; set; }
        public Gamedata gameData { get; set; }
        public Livedata liveData { get; set; }


        public class Metadata
        {
            public int wait { get; set; }
            public string timeStamp { get; set; }
        }

        public class Gamedata
        {
            public Game game { get; set; }
            public Datetime datetime { get; set; }
            public Status status { get; set; }
            public Teams teams { get; set; }
            public Venue venue { get; set; }
        }

        public class Game
        {
            public int pk { get; set; }
            public string season { get; set; }
            public string type { get; set; }
        }

        public class Datetime
        {
            public DateTime dateTime { get; set; }
            public DateTime endDateTime { get; set; }
        }

        public class Status
        {
            public string abstractGameState { get; set; }
            public string codedGameState { get; set; }
            public string detailedState { get; set; }
            public string statusCode { get; set; }
            public bool startTimeTBD { get; set; }
        }

        public class Teams
        {
            public Away away { get; set; }
            public Home home { get; set; }
        }

        public class Away
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public Venue venue { get; set; }
            public string abbreviation { get; set; }
            public string triCode { get; set; }
            public string teamName { get; set; }
            public string locationName { get; set; }
            public string firstYearOfPlay { get; set; }
            public Division division { get; set; }
            public Conference conference { get; set; }
            public Franchise franchise { get; set; }
            public string shortName { get; set; }
            public string officialSiteUrl { get; set; }
            public int franchiseId { get; set; }
            public bool active { get; set; }
        }

        public class Timezone
        {
            public string id { get; set; }
            public int offset { get; set; }
            public string tz { get; set; }
        }

        public class Division
        {
            public int id { get; set; }
            public string name { get; set; }
            public string nameShort { get; set; }
            public string link { get; set; }
            public string abbreviation { get; set; }
        }

        public class Conference
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
        }

        public class Franchise
        {
            public int franchiseId { get; set; }
            public string teamName { get; set; }
            public string link { get; set; }
        }

        public class Home
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public Venue venue { get; set; }
            public string abbreviation { get; set; }
            public string triCode { get; set; }
            public string teamName { get; set; }
            public string locationName { get; set; }
            public string firstYearOfPlay { get; set; }
            public Division division { get; set; }
            public Conference conference { get; set; }
            public Franchise franchise { get; set; }
            public string shortName { get; set; }
            public string officialSiteUrl { get; set; }
            public int franchiseId { get; set; }
            public bool active { get; set; }
        }

        public class Venue
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string city { get; set; }
            public Timezone timeZone { get; set; }
        }

        public class Livedata
        {
            public Plays plays { get; set; }
            public Linescore linescore { get; set; }
            public Boxscore boxscore { get; set; }
            public Decisions decisions { get; set; }
        }

        public class Plays
        {
            public Allplay[] allPlays { get; set; }
            public int[] scoringPlays { get; set; }
            public int[] penaltyPlays { get; set; }
            public Playsbyperiod[] playsByPeriod { get; set; }
        }

        public class About
        {
            public int eventIdx { get; set; }
            public int eventId { get; set; }
            public int period { get; set; }
            public string periodType { get; set; }
            public string ordinalNum { get; set; }
            public string periodTime { get; set; }
            public string periodTimeRemaining { get; set; }
            public DateTime dateTime { get; set; }
            public Goals goals { get; set; }
        }

        public class Goals
        {
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Allplay
        {
            public Result result { get; set; }
            public AboutPlay about { get; set; }
            public PlayerInfo[] players { get; set; }
            public TeamData team { get; set; }
        }

        public class Result
        {
            public string _event { get; set; }
            public string eventCode { get; set; }
            public string eventTypeId { get; set; }
            public string description { get; set; }
            public string secondaryType { get; set; }
            public Strength strength { get; set; }
            public bool gameWinningGoal { get; set; }
            public bool emptyNet { get; set; }
            public string penaltySeverity { get; set; }
            public int penaltyMinutes { get; set; }
        }

        public class Strength
        {
            public string code { get; set; }
            public string name { get; set; }
        }

        public class AboutPlay
        {
            public int eventIdx { get; set; }
            public int eventId { get; set; }
            public int period { get; set; }
            public string periodType { get; set; }
            public string ordinalNum { get; set; }
            public string periodTime { get; set; }
            public string periodTimeRemaining { get; set; }
            public DateTime dateTime { get; set; }
            public Goals goals { get; set; }
        }

        public class TeamData
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string triCode { get; set; }
        }

        public class PlayerInfo
        {
            public PlayerData player { get; set; }
            public string playerType { get; set; }
            public int seasonTotal { get; set; }
        }

        public class PlayerData
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public string link { get; set; }
        }

        public class Playsbyperiod
        {
            public int startIndex { get; set; }
            public int[] plays { get; set; }
            public int endIndex { get; set; }
        }

        public class Linescore
        {
            public int currentPeriod { get; set; }
            public string currentPeriodOrdinal { get; set; }
            public string currentPeriodTimeRemaining { get; set; }
            public Period[] periods { get; set; }
            public Shootoutinfo shootoutInfo { get; set; }
            public Teams teams { get; set; }
            public string powerPlayStrength { get; set; }
            public bool hasShootout { get; set; }
            public Intermissioninfo intermissionInfo { get; set; }
            public Powerplayinfo powerPlayInfo { get; set; }
        }

        public class Shootoutinfo
        {
            public ShoutoutTeam away { get; set; }
            public ShoutoutTeam home { get; set; }
            public DateTime startTime { get; set; }
        }

        public class ShoutoutTeam
        {
            public int scores { get; set; }
            public int attempts { get; set; }
        }

        public class Intermissioninfo
        {
            public int intermissionTimeRemaining { get; set; }
            public int intermissionTimeElapsed { get; set; }
            public bool inIntermission { get; set; }
        }

        public class Powerplayinfo
        {
            public int situationTimeRemaining { get; set; }
            public int situationTimeElapsed { get; set; }
            public bool inSituation { get; set; }
        }

        public class Period
        {
            public string periodType { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public int num { get; set; }
            public string ordinalNum { get; set; }
            public PeriodStats home { get; set; }
            public PeriodStats away { get; set; }
        }

        public class PeriodStats
        {
            public int goals { get; set; }
            public int shotsOnGoal { get; set; }
            public string rinkSide { get; set; }
        }

        public class Boxscore
        {
            public BoxscoreTeams teams { get; set; }
            public Official[] officials { get; set; }
        }

        public class BoxscoreTeams
        {
            public BoxscoreTeam away { get; set; }
            public BoxscoreTeam home { get; set; }
        }

        public class BoxscoreTeam
        {
            public Team team { get; set; }
            public Teamstats teamStats { get; set; }
            public Dictionary<string, PlayerBoxscore> players { get; set; }
            public int[] goalies { get; set; }
            public int[] skaters { get; set; }
            public object[] onIce { get; set; }
            public object[] onIcePlus { get; set; }
            public int[] scratches { get; set; }
            public object[] penaltyBox { get; set; }
            public Coach[] coaches { get; set; }
        }

        public class Teamstats
        {
            public Teamskaterstats teamSkaterStats { get; set; }
        }

        public class Teamskaterstats
        {
            public int goals { get; set; }
            public int pim { get; set; }
            public int shots { get; set; }
            public string powerPlayPercentage { get; set; }
            public float powerPlayGoals { get; set; }
            public float powerPlayOpportunities { get; set; }
            public string faceOffWinPercentage { get; set; }
            public int blocked { get; set; }
            public int takeaways { get; set; }
            public int giveaways { get; set; }
            public int hits { get; set; }
        }

        public class PlayerBoxscore
        {
            public Person person { get; set; }
            public string jerseyNumber { get; set; }
            public Position position { get; set; }
            public Stats stats { get; set; }
        }

        public class Person
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public string link { get; set; }
            public string shootsCatches { get; set; }
            public string rosterStatus { get; set; }
        }

        public class Position
        {
            public string code { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string abbreviation { get; set; }
        }

        public class Stats
        {
            public Goaliestats goalieStats { get; set; }
            public Skaterstats skaterStats { get; set; }
        }

        public class Goaliestats
        {
            public string timeOnIce { get; set; }
            public int assists { get; set; }
            public int goals { get; set; }
            public int pim { get; set; }
            public int shots { get; set; }
            public int saves { get; set; }
            public int powerPlaySaves { get; set; }
            public int shortHandedSaves { get; set; }
            public int evenSaves { get; set; }
            public int shortHandedShotsAgainst { get; set; }
            public int evenShotsAgainst { get; set; }
            public int powerPlayShotsAgainst { get; set; }
            public string decision { get; set; }
            public float savePercentage { get; set; }
            public float powerPlaySavePercentage { get; set; }
            public float shortHandedSavePercentage { get; set; }
            public float evenStrengthSavePercentage { get; set; }
        }

        public class Skaterstats
        {
            public string timeOnIce { get; set; }
            public int assists { get; set; }
            public int goals { get; set; }
            public int shots { get; set; }
            public int hits { get; set; }
            public int powerPlayGoals { get; set; }
            public int powerPlayAssists { get; set; }
            public int penaltyMinutes { get; set; }
            public int faceOffWins { get; set; }
            public int faceoffTaken { get; set; }
            public int takeaways { get; set; }
            public int giveaways { get; set; }
            public int shortHandedGoals { get; set; }
            public int shortHandedAssists { get; set; }
            public int blocked { get; set; }
            public int plusMinus { get; set; }
            public string evenTimeOnIce { get; set; }
            public string powerPlayTimeOnIce { get; set; }
            public string shortHandedTimeOnIce { get; set; }
        }

        public class Coach
        {
            public CoachDetails person { get; set; }
            public Position position { get; set; }
        }

        public class CoachDetails
        {
            public string fullName { get; set; }
            public string link { get; set; }
        }

        public class Official
        {
            public OfficialData official { get; set; }
            public string officialType { get; set; }
        }

        public class OfficialData
        {
            public int id { get; set; }
            public string fullName { get; set; }
            public string link { get; set; }
        }

        public class Decisions
        {
            public PlayerDecision winner { get; set; }
            public PlayerDecision loser { get; set; }
            public PlayerDecision firstStar { get; set; }
            public PlayerDecision secondStar { get; set; }
            public PlayerDecision thirdStar { get; set; }
        }
    }
}
