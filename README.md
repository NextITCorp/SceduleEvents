# SceduleEvents
Temporal expressions, date ranges, and more for C#

This repor provides source code for building temporal expressions and date rages in C#.  
Temporal Expressions as concieved in this library are inspired by Martin Fowler's PLOP 
presentation [here](http://martinfowler.com/apsupp/recurring.pdf).  

The expression language Martin describes is fully implemented here, along with a few 
additions to make it possible to generate ranges of date/time and sequences of DateTime 
objects.

I've also included some extensino methods to support features on IEnumerable, ITemporalExpression,
DateTime, and DateRange.

###DateRange and DateIn
The DateRange Class provides a range of DateTimes that can be used to generate enumerable containers
of DateTime objects.  This could be convenient for, say populating a calendar from some scedule.
Some extensions are provided for checking if an enumerable range should be conditioned by a temporal 
expression.  for example, I can use te extension DateIn to condition an event to generate dateTime 
objects only for weekdays, or only for weekends, or only Mondays, etc.  within a range.

