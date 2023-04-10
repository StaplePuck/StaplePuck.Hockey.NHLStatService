using System;

namespace StaplePuck.Hockey.NHLStatService
{
    class Program
    {
        static void Main(string[] args)
        {
            var request = new DateRequest
            {
                //GameDateId = "2022-02-18",
                //GameDateId = "2022-09-25",
                //GameDateId = "2022-10-07",
                SeasonId = "20222023",
                GetTeamStates = true,
                IsPlayoffs = false
            };

            //Updater.UpdateDate(request);


            var updater = Updater.Init();
            //updater.Update();
            updater.UpdateRequest(request).Wait();
            //updater.UpdateDateRange(new DateTime(2019, 4, 10), new DateTime(2019, 5, 6));
        }
    }
}
