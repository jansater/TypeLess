using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace TypeLess
{
    public interface ITimeSpanAssertionU : IAssertionU<TimeSpan> {
        ITimeSpanAssertion StopIfNotValid { get; }
        ITimeSpanAssertionU Or(TimeSpan obj, string withName = null);
        new ITimeSpanAssertion IsTrue(Func<TimeSpan, bool> assertFunc, string msgIfFalse);
        new ITimeSpanAssertion IsFalse(Func<TimeSpan, bool> assertFunc, string msgIfTrue);
        new ITimeSpanAssertion IsNotEqualTo(TimeSpan comparedTo);
        new ITimeSpanAssertion IsEqualTo(TimeSpan comparedTo);
        ITimeSpanAssertion IsLongerThan(TimeSpan span);
        ITimeSpanAssertion IsShorterThan(TimeSpan span);
    }

    public interface ITimeSpanAssertion : ITimeSpanAssertionU, IAssertion<TimeSpan>
    {
       
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class TimeSpanAssertion : Assertion<TimeSpan>, ITimeSpanAssertion
    {
        private List<TimeSpanAssertion> _childAssertions = new List<TimeSpanAssertion>();

        public TimeSpanAssertion(string s, TimeSpan source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public ITimeSpanAssertion Combine(ITimeSpanAssertion otherAssertion)
        {
            return (ITimeSpanAssertion)base.Combine(otherAssertion);
        }

        public ITimeSpanAssertionU Or(TimeSpan obj, string withName = null)
        {
            Add(new TimeSpanAssertion(withName, obj, null, null, null));
            return this;
        }

        public new ITimeSpanAssertion StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new ITimeSpanAssertion IsTrue(Func<TimeSpan, bool> assertFunc, string msgIfFalse)
        {
            return (ITimeSpanAssertion)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new ITimeSpanAssertion IsFalse(Func<TimeSpan, bool> assertFunc, string msgIfTrue)
        {
            return (ITimeSpanAssertion)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new ITimeSpanAssertion IsNotEqualTo(TimeSpan comparedTo)
        {
            return (ITimeSpanAssertion)base.IsNotEqualTo(comparedTo);
        }

        public new ITimeSpanAssertion IsEqualTo(TimeSpan comparedTo)
        {
            return (ITimeSpanAssertion)base.IsEqualTo(comparedTo);
        }

        public ITimeSpanAssertion IsShorterThan(TimeSpan span)
        {
            Extend(x => x.Ticks < span.Ticks ? String.Format(CultureInfo.InvariantCulture, "must be longer than {0:c}", span) : null);

            return this;
        }

        public ITimeSpanAssertion IsLongerThan(TimeSpan span)
        {
            Extend(x => x.Ticks > span.Ticks ? String.Format(CultureInfo.InvariantCulture, "must be shorter than {0:c}", span) : null);
            return this;
        }

    }
}
