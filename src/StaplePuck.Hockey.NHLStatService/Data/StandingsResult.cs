using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    internal class StandingsResult
    {
        public Record[] records { get; set; }

        public class Record
        {
            public string standingsType { get; set; }
            public League league { get; set; }
            public Division division { get; set; }
            public Conference1 conference { get; set; }
            public string season { get; set; }
            public Teamrecord[] teamRecords { get; set; }
        }

        public class League
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
        }

        public class Division
        {
            public int id { get; set; }
            public string name { get; set; }
            public string nameShort { get; set; }
            public string link { get; set; }
            public string abbreviation { get; set; }
            public Conference conference { get; set; }
            public bool active { get; set; }
        }

        public class Conference
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
        }

        public class Conference1
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string abbreviation { get; set; }
            public string shortName { get; set; }
            public bool active { get; set; }
        }

        public class Teamrecord
        {
            public Team team { get; set; }
        }

        public class Team
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public Venue venue { get; set; }
            public string abbreviation { get; set; }
            public string teamName { get; set; }
            public string locationName { get; set; }
            public Nextschedule nextSchedule { get; set; }
        }

        public class Venue
        {
            public int id { get; set; }
            public string name { get; set; }
            public string link { get; set; }
            public string city { get; set; }
            public Timezone timeZone { get; set; }
        }

        public class Timezone
        {
            public string id { get; set; }
            public int offset { get; set; }
            public string tz { get; set; }
        }

        public class Nextschedule
        {
            public int totalItems { get; set; }
            public int totalEvents { get; set; }
            public int totalGames { get; set; }
            public int totalMatches { get; set; }
            public Metadata metaData { get; set; }
            public Date[] dates { get; set; }
        }

        public class Metadata
        {
            public string timeStamp { get; set; }
        }

        public class Date
        {
            public DateTime date { get; set; }
            public int totalItems { get; set; }
            public int totalEvents { get; set; }
            public int totalGames { get; set; }
            public int totalMatches { get; set; }
            public Game[] games { get; set; }
            public object[] events { get; set; }
            public object[] matches { get; set; }
        }

        public class Game
        {
            public int gamePk { get; set; }
            public string link { get; set; }
            public string gameType { get; set; }
            public string season { get; set; }
            public DateTime gameDate { get; set; }
            public Status status { get; set; }
        }

        public class Status
        {
            public string abstractGameState { get; set; }
            public string codedGameState { get; set; }
            public string detailedState { get; set; }
            public string statusCode { get; set; }
            public bool startTimeTBD { get; set; }
        }

    }
}
