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
                GameDateId = "2019-04-19",
                SeasonId = "20182019" 
            };

            Updater.UpdateDate(request);
        }
    }
}
