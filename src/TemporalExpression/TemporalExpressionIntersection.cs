using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TemporalExpression
{
    public class TemporalExpressionIntersection: ITemporalExpression, IEnumerable<ITemporalExpression>
    {
        private readonly List<ITemporalExpression> _list;

        public TemporalExpressionIntersection(IEnumerable<ITemporalExpression> data)
        {
            _list = data.ToList();
        }

        public bool Includes(DateTime dt)
        {
            return _list.All(item => item.Includes(dt));
        }

        public IEnumerator<ITemporalExpression> GetEnumerator()
        {
            return ((IEnumerable<ITemporalExpression>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
