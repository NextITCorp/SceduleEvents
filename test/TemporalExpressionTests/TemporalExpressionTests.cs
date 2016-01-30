using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DateExtensions;
using Moq;
using NUnit.Framework;
using TemporalExpression;

namespace TemporalExpressionTests
{
    [TestFixture]
    public class TemporalExpressionTests
    {
        [Test]
        public void temporal_expression_returns_boolean()
        {
            var m = Mock.Of<ITemporalExpression>(te => te.Includes(It.IsAny<DateTime>()));

            // boring
            Assert.That(m.Includes(DateTime.UtcNow), Is.EqualTo(true));
        }

        [Test]
        public void day_in_month_compares_to_date_as_expected()
        {
            // 2nd Monday of the month.
            var incudedDt = DateTime.SpecifyKind(new DateTime(2016, 01, 11), DateTimeKind.Utc);
            var excudedDt = DateTime.SpecifyKind(new DateTime(2016, 02, 10), DateTimeKind.Utc);
            var excluded2Dt = DateTime.SpecifyKind(new DateTime(2016, 02, 11), DateTimeKind.Utc);

            var te = new DayInMonth(incudedDt.WeekDayIndex(), incudedDt.Day / 7);

            Assert.That(te.Includes(incudedDt));
            Assert.That(te.Includes(excudedDt), Is.False);
            Assert.That(te.Includes(excluded2Dt), Is.False);
        }

        [Test]
        public void temporal_expression_sequence_matches_expected_dates()
        {
            // 2nd Monday and 2nd Thursday of the month.
            var te = new TemporalExpressionSequence(new[]
            {
                new DayInMonth(1, 1),
                new DayInMonth(4, 1) 
            });

            var incudedDt = DateTime.SpecifyKind(new DateTime(2016, 01, 11), DateTimeKind.Utc);
            var incuded2Dt = DateTime.SpecifyKind(new DateTime(2016, 02, 11), DateTimeKind.Utc);
            var excludedDt = DateTime.SpecifyKind(new DateTime(2016, 02, 10), DateTimeKind.Utc);

            Assert.That(te.Includes(incudedDt));
            Assert.That(te.Includes(incuded2Dt));
            Assert.That(te.Includes(excludedDt), Is.False);
        }

        [Test]
        public void temporal_expression_intersection_matches_expected_dates()
        {
            // 2nd Monday and 2nd Thursday of the month.
            var te = new TemporalExpressionIntersection(new List<ITemporalExpression>
            {
                new DayInMonth(1, 1),
                new RangeInYear(4, 1, 6, 30),  
            });

            var incudedDt = DateTime.SpecifyKind(new DateTime(2016, 05, 09), DateTimeKind.Utc);
            var incuded2Dt = DateTime.SpecifyKind(new DateTime(2016, 06, 13), DateTimeKind.Utc);
            var excludedDt = DateTime.SpecifyKind(new DateTime(2016, 02, 10), DateTimeKind.Utc);
            var excluded2Dt = DateTime.SpecifyKind(new DateTime(2016, 06, 11), DateTimeKind.Utc);

            Assert.That(te.Includes(incudedDt));
            Assert.That(te.Includes(incuded2Dt));
            Assert.That(te.Includes(excludedDt), Is.False);
            Assert.That(te.Includes(excluded2Dt), Is.False);
        }

        [Test]
        public void temporal_expression_difference_matches_expected_dates()
        {
            // include april through september excluding June
            var te = new TemporalExpressionDifference(new RangeInYear(4, 1, 9, 30), new RangeInYear(6, 1, 6, 30));

            var includedDt = DateTime.SpecifyKind(new DateTime(2016, 05, 09), DateTimeKind.Utc);
            var excludedDt = DateTime.SpecifyKind(new DateTime(2016, 06, 13), DateTimeKind.Utc);

            Assert.That(te.Includes(includedDt));
            Assert.That(te.Includes(excludedDt), Is.False);
        }

        [Test]
        public void date_in_generates_dates_in_temporal_expression()
        {
            // create a temporal expression over the first week of january excluding weekend days
            var te = new TemporalExpressionDifference(new RangeInYear(1, 1, 1, 7),
                new TemporalExpressionSequence(new[]
                {
                    new DayInMonth(0, 0), 
                    new DayInMonth(6, 0),
                }));
            // Apply this to a date range over the first full week of January.
            var firstDt = DateTime.SpecifyKind(new DateTime(2016, 01, 01), DateTimeKind.Utc);
            var lastDt = DateTime.SpecifyKind(new DateTime(2016, 01, 08), DateTimeKind.Utc);
            var lastDtInRange = lastDt - TimeSpan.FromDays(1);
            var included = te.DatesIn(new DateRange(firstDt, lastDt)).ToList();

            var count = included.Count();
            //foreach (var item in included)
            //{
            //    Console.WriteLine(item);
            //}

            // test a couple of things about the output
            // First entry should be a Friday
            Assert.That(included.First(), Is.EqualTo(firstDt));
            Assert.That(included.First().WeekDayIndex(), Is.EqualTo(firstDt.WeekDayIndex()));

            // Last day is a Thursday
            Assert.That(included.Last(), Is.EqualTo(lastDtInRange));
            Assert.That(included.Last().WeekDayIndex(), Is.EqualTo(lastDtInRange.WeekDayIndex()));

            // should be five days
            Assert.That(count, Is.EqualTo(5));
        }
    }
}
