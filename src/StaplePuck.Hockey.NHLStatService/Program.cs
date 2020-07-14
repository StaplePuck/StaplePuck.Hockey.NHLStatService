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
                GameDateId = "2019-04-21",
                SeasonId = "20182019" 
            };

            //Updater.UpdateDate(request);


            var updater = Updater.Init();
            updater.Update();
            //updater.UpdateDateRange(new DateTime(2019, 4, 10), new DateTime(2019, 5, 6));
        }
    }
}
