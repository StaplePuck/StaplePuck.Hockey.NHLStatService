using StaplePuck.Core;
using System;
using System.Text.RegularExpressions;

namespace StaplePuck.Hockey.NHLStatService
{
    class Program
    {
        static void Main(string[] args)
        {
            var startDate = DateTime.Parse("2026-04-08");
            //var endDate = DateTime.Parse("2025-10-08");
            var endDate = DateTime.Parse("2026-04-13");
            var currentDate = startDate;
            
            var updater = Updater.Init();
            
            while (currentDate <= endDate)
            {
                var gameDateId = currentDate.ToGameDateId();
                var request = new DateRequest
                {
                    //GameDateId = "2022-02-18",
                    //GameDateId = "2022-09-25",
                    //GameDateId = "2023-11-06",
                    GameDateId = gameDateId,
                    SeasonId = "20252026",
                    GetTeamStates = false,
                    IsPlayoffs = false
                };

                //Updater.UpdateDate(request);

                //updater.Update();
                updater.UpdateRequest(request).Wait();
                //updater.UpdateDateRange(new DateTime(2019, 4, 10), new DateTime(2019, 5, 6));
                currentDate = currentDate.AddDays(1);
            }
        }
    }
}
