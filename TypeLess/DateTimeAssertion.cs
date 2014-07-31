using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{
    public interface IDateTimeAssertionU : IAssertionU<DateTime>
    {
        IDateTimeAssertion IsNotWithin(DateTime min, DateTime max);
        IDateTimeAssertion IsWithin(DateTime min, DateTime max);
        IDateTimeAssertion IsBefore(DateTime date);
        IDateTimeAssertion IsAfter(DateTime date);
        IDateTimeAssertionU Or(DateTime obj, string withName = null);
        new IDateTimeAssertion IsTrue(Func<DateTime, bool> assertFunc, string errMsg = null);
        new IDateTimeAssertion IsFalse(Func<DateTime, bool> assertFunc, string errMsg = null);
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
            : base(s, source, file, lineNumber, caller) { }

        public IDateTimeAssertion Combine(IDateTimeAssertion otherAssertion)
        {
            return (IDateTimeAssertion)base.Or(otherAssertion);
        }

        public new IDateTimeAssertion StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public IDateTimeAssertion IsNotWithin(DateTime min, DateTime max)
        {
            Extend(x =>
            {
                dynamic d = Item;
                return AssertResult.New(d < min || d > max, Resources.IsNotWithin, min, max);
            });
            return this;
        }

        public IDateTimeAssertion IsWithin(DateTime min, DateTime max)
        {
            Extend(x =>
            {
                dynamic d = x;
                return AssertResult.New(d >= min && d <= max, Resources.IsWithin, min, max);
            });
            return this;
        }

        public IDateTimeAssertion IsBefore(DateTime date)
        {
            Extend(x => AssertResult.New(x.CompareTo(date) <= 0, Resources.IsBefore, date));
            return this;
        }

        public IDateTimeAssertion IsAfter(DateTime date)
        {
            Extend(x => AssertResult.New(x.CompareTo(date) >= 0, Resources.IsAfter, date));
            return this;
        }

        public IDateTimeAssertionU Or(DateTime obj, string withName = null)
        {
            AddWithOr(new DateTimeAssertion(withName, obj, null, null, null));
            return this;
        }

        public new IDateTimeAssertion IsTrue(Func<DateTime, bool> assertFunc, string errMsg = null)
        {
            return (IDateTimeAssertion)base.IsTrue(assertFunc, errMsg);
        }

        public new IDateTimeAssertion IsFalse(Func<DateTime, bool> assertFunc, string errMsg = null)
        {
            return (IDateTimeAssertion)base.IsFalse(assertFunc, errMsg);
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
            Extend(x => AssertResult.New((x.Date - date.Date).TotalDays == 0.0, Resources.SameDayAs, date.ToString("yyyy-MM-dd")));
            return this;
        }

        public IDateTimeAssertion SameMonthAs(DateTime date)
        {
            Extend(x =>
            {
                var sameYear = x.Date.Year == date.Year;
                var sameMonth = x.Date.Month == date.Month;

                return AssertResult.New(sameYear && sameMonth, Resources.SameMonthAs, date.ToString("yyyy-MM"));
            });
            return this;
        }

        public IDateTimeAssertion SameYearAs(DateTime date)
        {
            Extend(x =>
            {
                var sameYear = x.Date.Year == date.Year;
                return AssertResult.New(sameYear, Resources.SameYearAs, date.ToString("yyyy"));
            });
            return this;
        }

        public IDateTimeAssertion SameHourAs(DateTime date)
        {
            Extend(x =>
            {
                var diff = (x - date).TotalHours;
                return AssertResult.New(diff >= 0 && diff <= 1.0, Resources.SameHourAs, date.ToString("yyyy-MM-dd HH"));
            });
            return this;
        }

        public IDateTimeAssertion SameMinuteAs(DateTime date)
        {
            Extend(x =>
            {
                var diff = (Item - date).TotalMinutes;
                return AssertResult.New(diff >= 0 && diff <= 1.0, Resources.SameMinuteAs, date.ToString("yyyy-MM-dd HH:mm"));
            });
            return this;
        }

        public IDateTimeAssertion SameSecondAs(DateTime date)
        {
            Extend(x =>
            {
                var diff = (x - date).TotalSeconds;

                return AssertResult.New(diff >= 0 && diff <= 1.0, Resources.SameSecondAs, date.ToString("yyyy-MM-dd HH:mm:ss"));

            });
            return this;
        }

        public IDateTimeAssertion NotSameDayAs(DateTime date)
        {
            Extend(x => AssertResult.New((x.Date - date.Date).TotalDays != 0.0, Resources.NotSameDayAs, date.ToString("yyyy-MM-dd")));
            return this;
        }

        public IDateTimeAssertion NotSameMonthAs(DateTime date)
        {
            Extend(x =>
            {
                var sameYear = x.Date.Year == date.Year;
                var sameMonth = x.Date.Month == date.Month;

                return AssertResult.New(!(sameYear && sameMonth), Resources.NotSameMonthAs, date.ToString("yyyy-MM"));
            });
            return this;
        }

        public IDateTimeAssertion NotSameYearAs(DateTime date)
        {
            Extend(x =>
            {
                var sameYear = x.Date.Year == date.Year;

                return AssertResult.New(!sameYear, Resources.NotSameYearAs, date.ToString("yyyy"));
            });
            return this;
        }

        public IDateTimeAssertion NotSameHourAs(DateTime date)
        {
            Extend(x =>
            {
                var diff = (x - date).TotalHours;
                return AssertResult.New(diff < 0 || diff > 1.0, Resources.NotSameHourAs, date.ToString("yyyy-MM-dd HH"));
            });
            return this;
        }

        public IDateTimeAssertion NotSameMinuteAs(DateTime date)
        {
            Extend(x =>
            {
                var diff = (x - date).TotalMinutes;
                return AssertResult.New(diff < 0 || diff > 1.0, Resources.NotSameMinuteAs, date.ToString("yyyy-MM-dd HH:mm"));
            });
            return this;
        }

        public IDateTimeAssertion NotSameSecondAs(DateTime date)
        {
            Extend(x =>
            {
                var diff = (x - date).TotalSeconds;
                return AssertResult.New(diff < 0 || diff > 1.0, Resources.NotSameSecondAs, date.ToString("yyyy-MM-dd HH:mm:ss"));
            });
            return this;
        }


        public IDateTimeAssertion SameWeekDayAs(DateTime date)
        {
            Extend(x => AssertResult.New(date.DayOfWeek == date.DayOfWeek, Resources.SameWeekDayAs, date.DayOfWeek.ToString()));
            return this;
        }

        public IDateTimeAssertion NotSameWeekDayAs(DateTime date)
        {
            Extend(x => AssertResult.New(date.DayOfWeek != date.DayOfWeek, Resources.NotSameWeekDayAs, date.DayOfWeek.ToString()));
            return this;
        }
    }
}
