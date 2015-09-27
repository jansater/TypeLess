using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{

    public interface INumberAssertionU<T> : IAssertionU<T> where T : struct, IComparable<T>
    {
        INumberAssertion<T> IsZero { get; }
        INumberAssertion<T> IsPositive { get; }
        INumberAssertion<T> IsNegative { get; }
        INumberAssertion<T> IsNotWithin(T min, T max);
        INumberAssertion<T> IsWithin(T min, T max);
        INumberAssertionU<T> Or(T obj, string withName = null);

        new INumberAssertion<T> IsTrue(Func<T, bool> assertFunc, string errMsg = null);
        new INumberAssertion<T> IsFalse(Func<T, bool> assertFunc, string errMsg = null);

        INumberAssertion<T> IsLessThan(T comparedTo);
        INumberAssertion<T> IsGreaterThan(T comparedTo);
        INumberAssertion<T> IsLessThanOrEqualTo(T comparedTo);
        INumberAssertion<T> IsGreaterThanOrEqualTo(T comparedTo);
        new INumberAssertion<T> IsNotEqualTo(T comparedTo);
        new INumberAssertion<T> IsEqualTo(T comparedTo);

        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        new INumberAssertion<T> EvalPositive { get; }
    }

    public interface INumberAssertion<T> : IAssertion<T>, INumberAssertionU<T> where T : struct, IComparable<T>
    {

    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class NumberAssertion<T> : Assertion<T>, INumberAssertion<T> where T : struct, IComparable<T>
    {
        private List<NumberAssertion<T>> _childAssertions = new List<NumberAssertion<T>>();

        public NumberAssertion(string s, T source, string file, int? lineNumber, string caller)
            : base(s, source, file, lineNumber, caller) { }

        
        public INumberAssertion<T> Combine(INumberAssertion<T> otherAssertion)
        {
            return (INumberAssertion<T>)base.Or(otherAssertion);
        }

        public new INumberAssertion<T> StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public INumberAssertion<T> IsZero
        {
            get
            {
                Extend(x =>
                {
                    dynamic d = x;
                    return AssertResult.New(d == 0, Resources.IsZero);
                });
                return this;
            }

        }

        public INumberAssertion<T> IsPositive
        {
            get
            {
                Extend(x =>
                {
                    dynamic d = x;
                    return AssertResult.New(d > 0.0, Resources.IsPositive);
                });

                return this;
            }

        }

        public INumberAssertion<T> IsNegative
        {
            get
            {
                Extend(x =>
                {
                    dynamic d = x;
                    return AssertResult.New(d < 0.0, Resources.IsNegative);
                });
                return this;
            }

        }

        public INumberAssertion<T> IsNotWithin(T min, T max)
        {
            Extend(x =>
            {
                dynamic d = x;
                return AssertResult.New(d < min || d > max, Resources.IsNotWithin, min, max);
            });
            return this;
        }

        public INumberAssertion<T> IsWithin(T min, T max)
        {
            Extend(x =>
            {
                dynamic d = x;
                return AssertResult.New(d >= min && d <= max, Resources.IsWithin, min, max);
            });

            return this;
        }

        public INumberAssertion<T> IsLessThan(T comparedTo)
        {
            Extend(x => AssertResult.New(x.CompareTo(comparedTo) < 0, Resources.IsSmallerThan, comparedTo));

            return this;
        }

        public INumberAssertion<T> IsGreaterThan(T comparedTo)
        {
            Extend(x => AssertResult.New(x.CompareTo(comparedTo) > 0, Resources.IsGreaterThan, comparedTo));
            return this;
        }

        public INumberAssertion<T> IsLessThanOrEqualTo(T comparedTo)
        {
            Extend(x => AssertResult.New(x.CompareTo(comparedTo) <= 0, Resources.IsSmallerThan, comparedTo));

            return this;
        }

        public INumberAssertion<T> IsGreaterThanOrEqualTo(T comparedTo)
        {
            Extend(x => AssertResult.New(x.CompareTo(comparedTo) >= 0, Resources.IsGreaterThan, comparedTo));
            return this;
        }

        public INumberAssertionU<T> Or(T obj, string withName = null)
        {
            AddWithOr(new NumberAssertion<T>(withName, obj, null, null, null));
            return this;
        }

        public new INumberAssertion<T> IsTrue(Func<T, bool> assertFunc, string errMsg = null)
        {
            return (INumberAssertion<T>)base.IsTrue(assertFunc, errMsg);
        }

        public new INumberAssertion<T> IsFalse(Func<T, bool> assertFunc, string errMsg = null)
        {
            return (INumberAssertion<T>)base.IsFalse(assertFunc, errMsg);
        }

        public new INumberAssertion<T> IsNotEqualTo(T comparedTo)
        {
            return (INumberAssertion<T>)base.IsNotEqualTo(comparedTo);
        }

        public new INumberAssertion<T> IsEqualTo(T comparedTo)
        {
            return (INumberAssertion<T>)base.IsEqualTo(comparedTo);
        }

        public new INumberAssertion<T> EvalPositive
        {
            get { return (INumberAssertion<T>)base.EvalPositive; }
        }
    }
}
