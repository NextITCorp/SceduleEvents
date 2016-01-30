using System;
using System.Linq;
using DateExtensions;

using NUnit.Framework;

namespace DateExpressionTests
{
    [TestFixture]
    public class DateExpressionTests
    {
        [Test]
        public void find_the_second_tuesday_of_January_2010()
        {
            var expected = DateTime.SpecifyKind(new DateTime(2010, 01, 12), DateTimeKind.Utc);

            const int weeks = 2;
            var day = DateHelpers.DateOfMonth(weeks, DayOfWeek.Tuesday, 01, 2010);

            Assert.That(expected, Is.EqualTo(day));

        }

        [Test]
        public void find_the_second_tuesday_of_February_2010()
        {
            var expected = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);

            const int weeks = 2;
            var day = DateHelpers.DateOfMonth(weeks, DayOfWeek.Tuesday, 02, 2010);

            Assert.That(expected, Is.EqualTo(day));
        }

        [Test]
        public void date_of_month_throws_on_month_out_of_range()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DateHelpers.DateOfMonth(3, DayOfWeek.Tuesday, -1, 2010));
            Assert.Throws<ArgumentOutOfRangeException>(() => DateHelpers.DateOfMonth(3, DayOfWeek.Tuesday, 13, 2010));
        }

        [Test]
        public void date_of_month_throws_on_week_out_of_range()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DateHelpers.DateOfMonth(-7, DayOfWeek.Tuesday, 02, 2010));
            Assert.Throws<ArgumentOutOfRangeException>(() => DateHelpers.DateOfMonth(7, DayOfWeek.Tuesday, 02, 2010));
        }

        [Test]
        public void is_utc_checks_a_date_for_utc_kind()
        {
            var utcDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var otherKindOfDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Unspecified);
            var yetAnotherDate = new DateTime(2010, 02, 09);

            Assert.That(utcDate.IsUtc());
            Assert.That(otherKindOfDate.IsUtc(), Is.EqualTo(false));
            Assert.That(yetAnotherDate.IsUtc(), Is.EqualTo(false));
        }

        [Test]
        public void date_range_constructor_throws_on_non_utc_start_date()
        {
            var aDate = new DateTime(2010, 02, 09);

            Assert.Throws<ArgumentException>(()=> new DateRange(aDate, new TimeSpan()));
        }

        [Test]
        public void date_range_constructor_throws_on_non_utc_start_or_end_date()
        {
            var startDate = new DateTime(2010, 02, 09);
            var endDate = new DateTime(2010, 02, 09);

            Assert.Throws<ArgumentException>(() => new DateRange(startDate, endDate));
        }

        [Test]
        public void date_range_constructor_throws_on_end_date_before_start_date()
        {
            var startDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(2010, 02, 07), DateTimeKind.Utc);

            Assert.Throws<ArgumentException>(() => new DateRange(startDate, endDate));
        }

        [Test]
        public void date_range_end_is_reproduced_successfully()
        {
            var startDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(2011, 02, 07), DateTimeKind.Utc);
            var ob = new DateRange(startDate, endDate);

            var expected = endDate;


            Assert.That(expected, Is.EqualTo(ob.End));
        }

        [Test]
        public void date_range_is_iterable()
        {
            var startDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(2010, 02, 10), DateTimeKind.Utc);
            var ob = new DateRange(startDate, endDate);

            var expected = (endDate - startDate).Days; 

            var actual = ob.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void date_range_iteration_includes_zero_interval()
        {
            var startDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var ob = new DateRange(startDate, endDate);

            var expected = (endDate - startDate).Days; // we exclude the last day

            var actual = ob.Count();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void date_time_range_enumerable_enumerates_6_hour_intervals()
        {
            var startDate = DateTime.SpecifyKind(new DateTime(2010, 02, 09), DateTimeKind.Utc);
            var endDate = DateTime.SpecifyKind(new DateTime(2010, 02, 10), DateTimeKind.Utc);
            var ob = new DateRange(startDate, endDate); // exclude end date

            var expected = (endDate - startDate).Days * 4; // we exclude the last day as well

            var actual = ob.GetEnumerable(TimeSpan.FromHours(6)).Count();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
