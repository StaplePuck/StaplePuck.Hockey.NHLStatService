using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaplePuck.Hockey.NHLStatService.Request
{
    public class Season
    {
        public string ExternalId { get; set; } = string.Empty;
        public bool IsPlayoffs { get; set; }
    }
}
