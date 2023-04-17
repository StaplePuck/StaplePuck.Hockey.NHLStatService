using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Request
{
    public class GameDateSeason
    {
        public string GameDateId { get; set; } = string.Empty;
        public Season Season { get; set; } = new Season();
    }
}
