using System;

namespace DateExtensions
{
    public static class DateHelpers
    {
        /// <summary>
        /// Use Date of month to get datetime objects for expressions like "the first 
        /// saturday of february 2016."  Note that you can acomplish "last tuesday of december 
        /// by setting weekindex to -1.
        /// </summary>
        /// <param name="weekIndex">"first" week  = 1, "second" week = 2, etc.</param>
        /// <param name="dayOfWeek">date enumeration</param>
        /// <param name="month">integer enumeration of the month starting at 0</param>
        /// <param name="year">integer year</param>
        /// <returns>A UTC-based DateTime object containing the requested date</returns>
        public static DateTime DateOfMonth(int weekIndex, DayOfWeek dayOfWeek, int month, int year)
        {
            // check week index
            if (weekIndex < -1) throw (new ArgumentOutOfRangeException(String.Format("weekIndex {0} out of range.  Must be at least -1 ", weekIndex)));
            if (weekIndex > Math.Ceiling(DateTime.DaysInMonth(year, month) / 7.0))
                throw (new ArgumentOutOfRangeException(String.Format("weekIndex {0} out of range.  Must be less than the number of weeks in the month", weekIndex)));

            var dayIndex = (int) dayOfWeek;
            var firstDayOfMonth = DateTime.SpecifyKind(new DateTime(year, month, 01), DateTimeKind.Utc);

            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            var diff = dayIndex - firstDayOfWeek;
            weekIndex = (diff < 0) ? weekIndex : weekIndex - 1;
            return firstDayOfMonth.AddDays(weekIndex * 7 + diff);
        }

        public static bool IsUtc(this DateTime dt)
        {
            return dt.Kind == DateTimeKind.Utc;
        }

        public static int WeekDayIndex(this DateTime dt)
        {
            return (int) dt.DayOfWeek;
        }
    }
}
