using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{
    public interface ITimeSpanAssertionU : IAssertionU<TimeSpan>
    {
        ITimeSpanAssertion StopIfNotValid { get; }
        ITimeSpanAssertionU Or(TimeSpan obj, string withName = null);
        new ITimeSpanAssertion IsTrue(Func<TimeSpan, bool> assertFunc, string errMsg = null);
        new ITimeSpanAssertion IsFalse(Func<TimeSpan, bool> assertFunc, string errMsg = null);
        new ITimeSpanAssertion IsNotEqualTo(TimeSpan comparedTo);
        new ITimeSpanAssertion IsEqualTo(TimeSpan comparedTo);
        ITimeSpanAssertion IsLongerThan(TimeSpan span);
        ITimeSpanAssertion IsShorterThan(TimeSpan span);
        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        new ITimeSpanAssertion EvalPositive { get; }
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
            : base(s, source, file, lineNumber, caller) { }

        public ITimeSpanAssertion Combine(ITimeSpanAssertion otherAssertion)
        {
            return (ITimeSpanAssertion)base.Or(otherAssertion);
        }

        public ITimeSpanAssertionU Or(TimeSpan obj, string withName = null)
        {
            AddWithOr(new TimeSpanAssertion(withName, obj, null, null, null));
            return this;
        }

        public new ITimeSpanAssertion StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new ITimeSpanAssertion IsTrue(Func<TimeSpan, bool> assertFunc, string errMsg = null)
        {
            return (ITimeSpanAssertion)base.IsTrue(assertFunc, errMsg);
        }

        public new ITimeSpanAssertion IsFalse(Func<TimeSpan, bool> assertFunc, string errMsg = null)
        {
            return (ITimeSpanAssertion)base.IsFalse(assertFunc, errMsg);
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
            Extend(x => AssertResult.New(x.Ticks < span.Ticks, Resources.TimeSpanIsShorterThan, span));
            return this;
        }

        public ITimeSpanAssertion IsLongerThan(TimeSpan span)
        {
            Extend(x => AssertResult.New(x.Ticks > span.Ticks, Resources.TimeSpanIsLongerThan, span));
            return this;
        }

        public new ITimeSpanAssertion EvalPositive
        {
            get { return (ITimeSpanAssertion)base.EvalPositive; }
        }
    }
}
