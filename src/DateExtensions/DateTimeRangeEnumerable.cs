using System;
using System.Collections.Generic;

namespace DateExtensions
{
    public class DateTimeRangeEnumerable : DateRange
    {
        private readonly TimeSpan _interval;
        public DateTimeRangeEnumerable(DateTime start, TimeSpan length, TimeSpan interval) : base(start, length)
        {
            _interval = interval;
        }

        public DateTimeRangeEnumerable(DateTime start, DateTime end, TimeSpan interval) : base(start, end)
        {
            _interval = interval;
        }

        public override IEnumerator<DateTime> GetEnumerator()
        {
        //    yield return Start; // include the first tic

            var i = _interval;
            var end = Start + _length;
            for (var iter = Start; iter < end; iter += i)
            {
                yield return iter;
            }
        }
    }
}