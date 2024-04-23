using System;
using System.Collections.Generic;
using System.Text;
using static StaplePuck.Hockey.NHLStatService.Data.LiveResult;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    internal class LiveResult
    {
        public string copyright { get; set; } = string.Empty;
        public int gamePk { get; set; }
        public string link { get; set; } = string.Empty;
        public Metadata? metaData { get; set; }
        public Gamedata? gameData { get; set; }
        public Livedata? liveData { get; set; }


        public class Metadata
        {
            public int wait { get; set; }
            public string timeStamp { get; set; } = string.Empty;
        }

        public class Gamedata
        {
            public Game? game { get; set; }
            public Datetime? datetime { get; set; }
            public Status? status { get; set; }
            public Teams? teams { get; set; }
            public Venue? venue { get; set; }
        }

        public class Game
        {
            public int pk { get; set; }
            public string season { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
        }

        public class Datetime
        {
            public DateTime? dateTime { get; set; }
            public DateTime? endDateTime { get; set; }
        }

        public class Status
        {
            public string abstractGameState { get; set; } = string.Empty;
            public string codedGameState { get; set; } = string.Empty;
            public string detailedState { get; set; } = string.Empty;
            public string statusCode { get; set; } = string.Empty;
            public bool startTimeTBD { get; set; }
        }

        public class Teams
        {
            public Away? away { get; set; }
            public Home? home { get; set; }
        }

        public class Away
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public Venue? venue { get; set; }
            public string abbreviation { get; set; } = string.Empty;
            public string triCode { get; set; } = string.Empty;
            public string teamName { get; set; } = string.Empty;
            public string locationName { get; set; } = string.Empty;
            public string firstYearOfPlay { get; set; } = string.Empty;
            public Division? division { get; set; }
            public Conference? conference { get; set; }
            public Franchise? franchise { get; set; }
            public string shortName { get; set; } = string.Empty;
            public string officialSiteUrl { get; set; } = string.Empty;
            public int franchiseId { get; set; }
            public bool active { get; set; }
        }

        public class Timezone
        {
            public string id { get; set; } = string.Empty;
            public int offset { get; set; }
            public string tz { get; set; } = string.Empty;
        }

        public class Division
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string nameShort { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string abbreviation { get; set; } = string.Empty;
        }

        public class Conference
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }

        public class Franchise
        {
            public int franchiseId { get; set; }
            public string teamName { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }

        public class Home
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public Venue? venue { get; set; }
            public string abbreviation { get; set; } = string.Empty;
            public string triCode { get; set; } = string.Empty;
            public string teamName { get; set; } = string.Empty;
            public string locationName { get; set; } = string.Empty;
            public string firstYearOfPlay { get; set; } = string.Empty;
            public Division? division { get; set; }
            public Conference? conference { get; set; }
            public Franchise? franchise { get; set; }
            public string shortName { get; set; } = string.Empty;
            public string officialSiteUrl { get; set; } = string.Empty;
            public int franchiseId { get; set; }
            public bool active { get; set; }
        }

        public class Venue
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string city { get; set; } = string.Empty;
            public Timezone? timeZone { get; set; }
        }

        public class Livedata
        {
            public Plays? plays { get; set; }
            public Linescore? linescore { get; set; }
            public Boxscore? boxscore { get; set; }
        }

        public class Plays
        {
            public Allplay[] allPlays { get; set; } = Array.Empty<Allplay>();
            public int[] scoringPlays { get; set; } = Array.Empty<int>();
            public int[] penaltyPlays { get; set; } = Array.Empty<int>();
            public Playsbyperiod[] playsByPeriod { get; set; } = Array.Empty<Playsbyperiod>();
        }

        public class About
        {
            public int eventIdx { get; set; }
            public int eventId { get; set; }
            public int period { get; set; }
            public string periodType { get; set; } = string.Empty;
            public string ordinalNum { get; set; } = string.Empty;
            public string periodTime { get; set; } = string.Empty;
            public string periodTimeRemaining { get; set; } = string.Empty;
            public DateTime? dateTime { get; set; }
            public Goals? goals { get; set; }
        }

        public class Goals
        {
            public int away { get; set; }
            public int home { get; set; }
        }

        public class Allplay
        {
            public Result? result { get; set; }
            public AboutPlay? about { get; set; }
            public PlayerInfo[] players { get; set; } = Array.Empty<PlayerInfo>();
            public TeamData? team { get; set; }
        }

        public class Result
        {
            public string _event { get; set; } = string.Empty;
            public string eventCode { get; set; } = string.Empty;
            public string eventTypeId { get; set; } = string.Empty;
            public string description { get; set; } = string.Empty;
            public string secondaryType { get; set; } = string.Empty;
            public Strength? strength { get; set; }
            public bool gameWinningGoal { get; set; }
            public bool emptyNet { get; set; }
            public string penaltySeverity { get; set; } = string.Empty;
            public int penaltyMinutes { get; set; }
        }

        public class Strength
        {
            public string code { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
        }

        public class AboutPlay
        {
            public int eventIdx { get; set; }
            public int eventId { get; set; }
            public int period { get; set; }
            public string periodType { get; set; } = string.Empty;
            public string ordinalNum { get; set; } = string.Empty;
            public string periodTime { get; set; } = string.Empty;
            public string periodTimeRemaining { get; set; } = string.Empty;
            public DateTime? dateTime { get; set; }
            public Goals? goals { get; set; }
        }

        public class TeamData
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string triCode { get; set; } = string.Empty;
        }

        public class PlayerInfo
        {
            public PlayerData? player { get; set; }
            public string playerType { get; set; } = string.Empty;
            public int seasonTotal { get; set; }
        }

        public class PlayerData
        {
            public int id { get; set; }
            public string fullName { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }

        public class Playsbyperiod
        {
            public int startIndex { get; set; }
            public int[] plays { get; set; } = Array.Empty<int>();
            public int endIndex { get; set; }
        }

        public class Linescore
        {
            public int currentPeriod { get; set; }
            public string currentPeriodOrdinal { get; set; } = string.Empty;
            public string currentPeriodTimeRemaining { get; set; } = string.Empty;
            public Period[] periods { get; set; } = Array.Empty<Period>();
            public Shootoutinfo? shootoutInfo { get; set; }
            public Teams? teams { get; set; }
            public string powerPlayStrength { get; set; } = string.Empty;
            public bool hasShootout { get; set; }
            public Intermissioninfo? intermissionInfo { get; set; }
            public Powerplayinfo? powerPlayInfo { get; set; }
        }

        public class Shootoutinfo
        {
            public ShoutoutTeam? away { get; set; }
            public ShoutoutTeam? home { get; set; }
            public DateTime? startTime { get; set; }
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
            public string periodType { get; set; } = string.Empty;
            public DateTime? startTime { get; set; }
            public DateTime? endTime { get; set; }
            public int num { get; set; }
            public string ordinalNum { get; set; } = string.Empty;
            public PeriodStats? home { get; set; }
            public PeriodStats? away { get; set; }
        }

        public class PeriodStats
        {
            public int goals { get; set; }
            public int shotsOnGoal { get; set; }
            public string rinkSide { get; set; } = string.Empty;
        }

        public class Boxscore
        {
            public BoxscoreTeams? teams { get; set; }
            public Official[] officials { get; set; } = Array.Empty<Official>();
        }

        public class BoxscoreTeams
        {
            public BoxscoreTeam? away { get; set; }
            public BoxscoreTeam? home { get; set; }
        }

        public class BoxscoreTeam
        {
            public Team? team { get; set; }
            public Teamstats? teamStats { get; set; }
            public Dictionary<string, PlayerBoxscore> players { get; set; } = new Dictionary<string, PlayerBoxscore>();
            public int[] goalies { get; set; } = Array.Empty<int>();
            public int[] skaters { get; set; } = Array.Empty<int>();
            public int[] scratches { get; set; } = Array.Empty<int>();
            public Coach[] coaches { get; set; } = Array.Empty<Coach>();
        }

        public class Teamstats
        {
            public Teamskaterstats? teamSkaterStats { get; set; }
        }

        public class Teamskaterstats
        {
            public int goals { get; set; }
            public int pim { get; set; }
            public int shots { get; set; }
            public string powerPlayPercentage { get; set; } = string.Empty;
            public float powerPlayGoals { get; set; }
            public float powerPlayOpportunities { get; set; }
            public string faceOffWinPercentage { get; set; } = string.Empty;
            public int blocked { get; set; }
            public int takeaways { get; set; }
            public int giveaways { get; set; }
            public int hits { get; set; }
        }

        public class PlayerBoxscore
        {
            public Person? person { get; set; }
            public string jerseyNumber { get; set; } = string.Empty;
            public Position? position { get; set; }
            public Stats? stats { get; set; }
        }

        public class Person
        {
            public int id { get; set; }
            public string fullName { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string shootsCatches { get; set; } = string.Empty;
            public string rosterStatus { get; set; } = string.Empty;
        }

        public class Position
        {
            public string code { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
            public string type { get; set; } = string.Empty;
            public string abbreviation { get; set; } = string.Empty;
        }

        public class Stats
        {
            public Goaliestats? goalieStats { get; set; }
            public Skaterstats? skaterStats { get; set; }
        }

        public class Goaliestats
        {
            public string timeOnIce { get; set; } = string.Empty;
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
            public string decision { get; set; } = string.Empty;
            public float savePercentage { get; set; }
            public float powerPlaySavePercentage { get; set; }
            public float shortHandedSavePercentage { get; set; }
            public float evenStrengthSavePercentage { get; set; }
        }

        public class Skaterstats
        {
            public string timeOnIce { get; set; } = string.Empty;
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
            public string evenTimeOnIce { get; set; } = string.Empty;
            public string powerPlayTimeOnIce { get; set; } = string.Empty;
            public string shortHandedTimeOnIce { get; set; } = string.Empty;
        }

        public class Coach
        {
            public CoachDetails? person { get; set; }
            public Position? position { get; set; }
        }

        public class CoachDetails
        {
            public string fullName { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }

        public class Official
        {
            public OfficialData? official { get; set; }
            public string officialType { get; set; } = string.Empty;
        }

        public class OfficialData
        {
            public int id { get; set; }
            public string fullName { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }
    }
}
