using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalExpression
{
    public class RangeInYear : ITemporalExpression
    {
        private int _startMonth;
        private int _startDay;

        private int _endMonth;
        private int _endDay;

        public RangeInYear(int startMonth, int startDay, int endMonth, int endDay)
        {
            _startMonth = startMonth;
            _startDay = startDay;
            _endMonth = endMonth;
            _endDay = endDay;
        }

        public bool Includes(DateTime dt)
        {
            return dt.Month >= _startMonth && dt.Month <= _endMonth
                   && dt.Day >= _startDay && dt.Day <= _endDay;
        }
    }
}
