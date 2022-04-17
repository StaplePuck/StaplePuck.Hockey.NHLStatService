using System;

namespace StaplePuck.Hockey.NHLStatService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var request = new DateRequest
            {
                //GameDateId = "2020-08-03",
                SeasonId = "20202021",
                GetTeamStates = false
            };

            //Updater.UpdateDate(request);


            var updater = Updater.Init();
            //updater.Update();
            updater.UpdateRequest(request).Wait();
            //updater.UpdateDateRange(new DateTime(2019, 4, 10), new DateTime(2019, 5, 6));
        }
    }
}
