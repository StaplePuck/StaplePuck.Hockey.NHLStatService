using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService
{
    public class DateRequest
    {
        public string SeasonId { get; set; }
        public string GameDateId { get; set; }
        public bool GetTeamStates { get; set; }

        public static DateRequest Deserialize(string text)
        {
            return JsonConvert.DeserializeObject<DateRequest>(text);
        }
    }
}
