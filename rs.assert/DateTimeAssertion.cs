using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace TypeLess
{
    public interface IDateTimeAssertionU : IAssertionU<DateTime>
    {
        IDateTimeAssertion IsNotWithin(DateTime min, DateTime max);
        IDateTimeAssertion IsWithin(DateTime min, DateTime max);
        IDateTimeAssertion IsBefore(DateTime date);
        IDateTimeAssertion IsAfter(DateTime date);
        IDateTimeAssertion Or(DateTime obj, string withName = null);
        new IDateTimeAssertion IsTrue(Func<DateTime, bool> assertFunc, string msgIfFalse);
        new IDateTimeAssertion IsFalse(Func<DateTime, bool> assertFunc, string msgIfTrue);
        new IDateTimeAssertion IsNotEqualTo(DateTime comparedTo);
        new IDateTimeAssertion IsEqualTo(DateTime comparedTo);
        
        IDateTimeAssertion SameDayAs(DateTime date);
        IDateTimeAssertion SameMonthAs(DateTime date);
        IDateTimeAssertion SameYearAs(DateTime date);
        IDateTimeAssertion SameHourAs(DateTime date);
        IDateTimeAssertion SameMinuteAs(DateTime date);
        IDateTimeAssertion SameSecondAs(DateTime date);

        IDateTimeAssertion NotSameDayAs(DateTime date);
        IDateTimeAssertion NotSameMonthAs(DateTime date);
        IDateTimeAssertion NotSameYearAs(DateTime date);
        IDateTimeAssertion NotSameHourAs(DateTime date);
        IDateTimeAssertion NotSameMinuteAs(DateTime date);
        IDateTimeAssertion NotSameSecondAs(DateTime date);

        IDateTimeAssertion SameWeekDayAs(DateTime date);
        IDateTimeAssertion NotSameWeekDayAs(DateTime date);
        
    }

    public interface IDateTimeAssertion : IDateTimeAssertionU, IAssertion<DateTime>
    {
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class DateTimeAssertion : Assertion<DateTime>, IDateTimeAssertion
    {
        private List<DateTimeAssertion> _childAssertions = new List<DateTimeAssertion>();

        public DateTimeAssertion(string s, DateTime source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        internal new List<DateTimeAssertion> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public IDateTimeAssertion Combine(IDateTimeAssertion otherAssertion)
        {
            return (IDateTimeAssertion)base.Combine(otherAssertion);
        }

        public new IDateTimeAssertion StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public IDateTimeAssertion IsNotWithin(DateTime min, DateTime max) 
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            dynamic d = Item;
            if (d < min || d > max)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be within {0} and {1}", min, max));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsNotWithin(min, max));
            }

            return this;
        }

        public IDateTimeAssertion IsWithin(DateTime min, DateTime max)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            dynamic d = Item;
            if (d >= min && d <= max)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be within {0} and {1}", min, max));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsWithin(min, max));
            }

            return this;
        }

        public IDateTimeAssertion IsBefore(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if (Item.CompareTo(date) <= 0)
            {
                Append("must be larger than " + date);
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsBefore(date));
            }

            return this;
        }

        public IDateTimeAssertion IsAfter(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if (Item.CompareTo(date) >= 0)
            {
                Append("must be smaller than " + date);
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsAfter(date));
            }

            return this;
        }

        public IDateTimeAssertion Or(DateTime obj, string withName = null)
        {
            this.ChildAssertions.Add(new DateTimeAssertion(withName, obj, null, null, null));
            return this; 
        }

        public new IDateTimeAssertion IsTrue(Func<DateTime, bool> assertFunc, string msgIfFalse)
        {
            return (IDateTimeAssertion)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new IDateTimeAssertion IsFalse(Func<DateTime, bool> assertFunc, string msgIfTrue)
        {
            return (IDateTimeAssertion)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new IDateTimeAssertion IsNotEqualTo(DateTime comparedTo)
        {
            return (IDateTimeAssertion)base.IsNotEqualTo(comparedTo);
        }

        public new IDateTimeAssertion IsEqualTo(DateTime comparedTo)
        {
            return (IDateTimeAssertion)base.IsEqualTo(comparedTo);
        }

        public IDateTimeAssertion SameDayAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if ((Item.Date - date.Date).TotalDays == 0.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same day as {0}", date.ToString("yyyy-MM-dd")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameDayAs(date));
            }
            return this;
        }

        public IDateTimeAssertion SameMonthAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var sameYear = Item.Date.Year == date.Year;
            var sameMonth = Item.Date.Month == date.Month;

            if (sameYear && sameMonth)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same month as {0}", date.ToString("yyyy-MM")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameMonthAs(date));
            }
            return this;
        }

        public IDateTimeAssertion SameYearAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var sameYear = Item.Date.Year == date.Year;
            
            if (sameYear)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same year as {0}", date.ToString("yyyy")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameYearAs(date));
            }
            return this;
        }

        public IDateTimeAssertion SameHourAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var diff = (Item - date).TotalHours;

            if (diff >= 0 && diff <= 1.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same hour as {0}", date.ToString("yyyy-MM-dd HH")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameHourAs(date));
            }
            return this;
        }

        public IDateTimeAssertion SameMinuteAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var diff = (Item - date).TotalMinutes;

            if (diff >= 0 && diff <= 1.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same minute as {0}", date.ToString("yyyy-MM-dd HH:mm")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameMinuteAs(date));
            }
            return this;
        }

        public IDateTimeAssertion SameSecondAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var diff = (Item - date).TotalSeconds;

            if (diff >= 0 && diff <= 1.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same second as {0}", date.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameSecondAs(date));
            }
            return this;
        }

        public IDateTimeAssertion NotSameDayAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if ((Item.Date - date.Date).TotalDays != 0.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same day as {0}", date.ToString("yyyy-MM-dd")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameDayAs(date));
            }
            return this;
        }

        public IDateTimeAssertion NotSameMonthAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var sameYear = Item.Date.Year == date.Year;
            var sameMonth = Item.Date.Month == date.Month;

            if (!(sameYear && sameMonth))
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same month as {0}", date.ToString("yyyy-MM")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameMonthAs(date));
            }
            return this;
        }

        public IDateTimeAssertion NotSameYearAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var sameYear = Item.Date.Year == date.Year;

            if (!sameYear)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same year as {0}", date.ToString("yyyy")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameYearAs(date));
            }
            return this;
        }

        public IDateTimeAssertion NotSameHourAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var diff = (Item - date).TotalHours;

            if (diff < 0 || diff > 1.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same hour as {0}", date.ToString("yyyy-MM-dd HH")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameHourAs(date));
            }
            return this;
        }

        public IDateTimeAssertion NotSameMinuteAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var diff = (Item - date).TotalMinutes;

            if (diff < 0 || diff > 1.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same minute as {0}", date.ToString("yyyy-MM-dd HH:mm")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameMinuteAs(date));
            }
            return this;
        }

        public IDateTimeAssertion NotSameSecondAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            var diff = (Item - date).TotalSeconds;

            if (diff < 0 || diff > 1.0)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same second as {0}", date.ToString("yyyy-MM-dd HH:mm:ss")));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameSecondAs(date));
            }
            return this;
        }


        public IDateTimeAssertion SameWeekDayAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if (date.DayOfWeek == date.DayOfWeek)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must not be on same week day as {0}", date.DayOfWeek.ToString()));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.SameWeekDayAs(date));
            }
            return this;
        }


        public IDateTimeAssertion NotSameWeekDayAs(DateTime date)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if (date.DayOfWeek != date.DayOfWeek)
            {
                Append(String.Format(CultureInfo.InvariantCulture, "must be on same week day as {0}", date.DayOfWeek.ToString()));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.NotSameWeekDayAs(date));
            }
            return this;
        }
    }
}
