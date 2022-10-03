using System;
using System.Collections.Generic;
using System.Text;

namespace StaplePuck.Hockey.NHLStatService
{
    public static class DateExtensions
    {
        public static bool IsToday2(this DateTime date)
        {
            var todaysDate = TodaysDate2();
            var newDate = date.Subtract(new TimeSpan(6, 0, 0));
            var value = newDate.Date.Equals(todaysDate.Date);
            return value;
        }

        public static DateTime TodaysDate2()
        {
            var date = DateTime.UtcNow;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || true)
            {
                if (date.Hour < 16)
                {
                    date = date.Subtract(new TimeSpan(1, 0, 0, 0));
                }
            }
            else
            {
                if (date.Hour < 20)
                {
                    date = date.Subtract(new TimeSpan(1, 0, 0, 0));
                }
            }
            return date;
        }
    }
}
