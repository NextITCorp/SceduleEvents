using System;

namespace TemporalExpression
{
    public interface ITemporalExpression
    {
        bool Includes(DateTime dt);
    }
}
