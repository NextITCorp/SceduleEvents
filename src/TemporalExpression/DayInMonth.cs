using System;
using DateExtensions;

namespace TemporalExpression
{
    public class DayInMonth : ITemporalExpression
    {
        private int _count;
        private int _dayIndex;

        public DayInMonth(int dayindex, int count)
        {
            _count = count;
            _dayIndex = dayindex;
        }

        public bool Includes(DateTime dt)
        {
            return DayMatches(dt) && WeekInMonthMatches(dt);
        }

        private bool DayMatches(DateTime dt)
        {
            //return dt.Day % 7 == _dayIndex;
            return dt.WeekDayIndex() == _dayIndex;
        }

        private bool WeekInMonthMatches(DateTime dt)
        {
            if (_count >= 0)
                return dt.Day / 7 == _count;
            return DateTime.DaysInMonth(dt.Year, dt.Month) / 7 + _count + 1 == dt.Day / 7;
        }
    }
}
