using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TypeLess
{
    public interface IDateTimeAssertionU : IAssertion<DateTime>
    {
        IDateTimeAssertion IsNotWithin(DateTime min, DateTime max);
        IDateTimeAssertion IsBefore(DateTime date);
        IDateTimeAssertion IsAfter(DateTime date);
        IDateTimeAssertion Or(DateTime obj, string withName = null);
        new IDateTimeAssertion IsTrue(Func<DateTime, bool> assertFunc, string msgIfFalse);
        new IDateTimeAssertion IsFalse(Func<DateTime, bool> assertFunc, string msgIfTrue);
        new IDateTimeAssertion IsNotEqualTo(DateTime comparedTo);
        new IDateTimeAssertion IsEqualTo(DateTime comparedTo);
    }

    public interface IDateTimeAssertion : IDateTimeAssertionU, ICompleteAssertion
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

        public IDateTimeAssertion StopIfNotValid
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
                Append(String.Format("must be within {0} and {1}", min, max));
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsNotWithin(min, max));
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

        public new IDateTimeAssertion Or(DateTime obj, string withName = null)
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
    }
}
