using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalExpression
{
    public class TemporalExpressionDifference : ITemporalExpression
    {
        private ITemporalExpression _included;
        private ITemporalExpression _excluded;

        public TemporalExpressionDifference(ITemporalExpression included, ITemporalExpression excluded)
        {
            _included = included;
            _excluded = excluded;
        }

        public bool Includes(DateTime dt)
        {
            return _included.Includes(dt) && !_excluded.Includes(dt);
        }
    }
}
