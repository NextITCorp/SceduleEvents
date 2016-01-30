using System;
using System.Collections.Generic;
using System.Linq;
using DateExtensions;

namespace TemporalExpression
{
    public static class TemporalExpressionExtensions
    {
        public static IEnumerable<DateTime> DatesIn(this ITemporalExpression expression, DateRange range)
        {
            return range.Where(expression.Includes);
        }
    }
}
