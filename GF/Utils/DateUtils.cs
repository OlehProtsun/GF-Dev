using System;
using System.Globalization;

namespace GF.Utils
{
    public static class DateUtils
    {
        private static readonly CultureInfo _en = new CultureInfo("en-US");

        /// <summary>
        /// Повертає дволітерну англійську абревіатуру дня тижня (Mo, Tu, We …).
        /// </summary>
        public static string GetDayAbbr(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            // "Mon", "Tue", … -> беремо перші 2 символи
            return _en.DateTimeFormat.AbbreviatedDayNames[(int)date.DayOfWeek].Substring(0, 2);
        }
    }
}
