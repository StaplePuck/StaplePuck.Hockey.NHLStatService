using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService.Data
{
    internal class StandingsResult
    {
        public Record[] records { get; set; } = Array.Empty<Record>();

        public class Record
        {
            public string standingsType { get; set; } = string.Empty;
            public League? league { get; set; }
            public Division? division { get; set; }
            public Conference1? conference { get; set; }
            public string season { get; set; } = string.Empty;
            public Teamrecord[] teamRecords { get; set; } = Array.Empty<Teamrecord>();
        }

        public class League
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }

        public class Division
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string nameShort { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string abbreviation { get; set; } = string.Empty;
            public Conference? conference { get; set; }
            public bool active { get; set; }
        }

        public class Conference
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
        }

        public class Conference1
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string abbreviation { get; set; } = string.Empty;
            public string shortName { get; set; } = string.Empty;
            public bool active { get; set; }
        }

        public class Teamrecord
        {
            public Team? team { get; set; }
        }

        public class Team
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public Venue? venue { get; set; }
            public string abbreviation { get; set; } = string.Empty;
            public string teamName { get; set; } = string.Empty;
            public string locationName { get; set; } = string.Empty;
            public Nextschedule? nextSchedule { get; set; }
        }

        public class Venue
        {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string link { get; set; } = string.Empty;
            public string city { get; set; } = string.Empty;
            public Timezone? timeZone { get; set; }
        }

        public class Timezone
        {
            public string id { get; set; } = string.Empty;
            public int offset { get; set; }
            public string tz { get; set; } = string.Empty;
        }

        public class Nextschedule
        {
            public int totalItems { get; set; }
            public int totalEvents { get; set; }
            public int totalGames { get; set; }
            public int totalMatches { get; set; }
            public Metadata? metaData { get; set; }
            public Date[] dates { get; set; } = Array.Empty<Date>();
        }

        public class Metadata
        {
            public string timeStamp { get; set; } = string.Empty;
        }

        public class Date
        {
            public DateTime? date { get; set; }
            public int totalItems { get; set; }
            public int totalEvents { get; set; }
            public int totalGames { get; set; }
            public int totalMatches { get; set; }
            public Game[] games { get; set; } = Array.Empty<Game>();
        }

        public class Game
        {
            public int gamePk { get; set; }
            public string link { get; set; } = string.Empty;
            public string gameType { get; set; } = string.Empty;
            public string season { get; set; } = string.Empty;
            public DateTime? gameDate { get; set; }
            public Status? status { get; set; }
        }

        public class Status
        {
            public string abstractGameState { get; set; } = string.Empty;
            public string codedGameState { get; set; } = string.Empty;
            public string detailedState { get; set; } = string.Empty;
            public string statusCode { get; set; } = string.Empty;
            public bool startTimeTBD { get; set; }
        }

    }
}
