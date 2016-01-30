using System;
using System.Collections;
using System.Collections.Generic;

namespace DateExtensions
{
    /// <summary>
    /// DateRange records a timespan with a start date that lets us create enumerable collections of dates.
    /// This can be useful for enumerating regularly spaced sets of dates or simply to determine if an 
    /// event occurs in a timespan.  Very handy in conjuction with TemporalExpressions to derive collections 
    /// of dates while taking into account some constraint on the date or time (i.e during working hours, or 
    /// only on the weekend).  Objects of this type should be immutable.
    /// </summary>
    public class DateRange : IEnumerable<DateTime>
    {
        /// <summary>
        /// start time of the range
        /// </summary>
        private DateTime _start;

        /// <summary>
        /// length of time the range lasts.
        /// </summary>
        protected TimeSpan _length;

        /// <summary>
        /// ctor.  Create a DateRange object
        /// </summary>
        /// <param name="start">The UTC start datetime of the range.  Must be a UTC datetime.</param>
        /// <param name="length">The timespan of the range</param>
        public DateRange(DateTime start, TimeSpan length)
        {
            if (!start.IsUtc()) throw new ArgumentException(String.Format("start date must be of utc kind: {0}", start.Kind));

            InnerDateRangeCtor(start, length);
        }

        /// <summary>
        /// ctor.  Create a DateRange object
        /// </summary>
        /// <param name="start">The UTC start datetime of the range.  Must be a UTC datetime.</param>
        /// <param name="end">The UTC end datetime of the range.  Must be a UTC datetime.  The endDate 
        /// is NOT included in the range.</param>
        public DateRange(DateTime start, DateTime end)
        {
            if (!start.IsUtc()) throw new ArgumentException(String.Format("start date must be of utc kind: {0}", start.Kind));
            if (!end.IsUtc()) throw new ArgumentException(String.Format("end date must be of utc kind: {0}", end.Kind));

            if (start > end) throw new ArgumentException(String.Format("end date {0} must be after start date {1}", end, start));

            InnerDateRangeCtor(start, (end - start));
        }

        private void InnerDateRangeCtor(DateTime start, TimeSpan length)
        {
            _start = start;
            _length = length;
        }


        public DateTime Start
        {
            get { return _start; }
        }

        public DateTime End
        {
            get { return Start + _length; }
        }

        /// <summary>
        /// Create an object from this daterange object that is enumerable over some timespan
        /// </summary>
        /// <param name="interval">the 'tic' for enumeration (for example 6 hours or 1 day or 5 minutes)</param>
        /// <returns></returns>
        public virtual DateTimeRangeEnumerable GetEnumerable(TimeSpan interval)
        {
            return new DateTimeRangeEnumerable(Start, _length, interval);
        }

        public virtual IEnumerator<DateTime> GetEnumerator()
        {
            var i = TimeSpan.FromDays(1);
            var end = Start + _length;
            for (var iter = Start; iter < end; iter += i)
            {
                yield return iter;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}