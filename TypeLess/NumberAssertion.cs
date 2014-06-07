using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using TypeLess.DataTypes;

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

        new INumberAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse);
        new INumberAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue);

        INumberAssertion<T> IsSmallerThan(T comparedTo);
        INumberAssertion<T> IsLargerThan(T comparedTo);
        new INumberAssertion<T> IsNotEqualTo(T comparedTo);
        new INumberAssertion<T> IsEqualTo(T comparedTo);
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
            return (INumberAssertion<T>)base.Combine(otherAssertion);
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
                    return AssertResult.New(d == 0, "<name> must be non zero");
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
                    return AssertResult.New(d > 0.0, "<name> must be zero or negative");
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
                    return AssertResult.New(d < 0.0, "<name> must be zero or positive");
                });
                return this;
            }

        }

        public INumberAssertion<T> IsNotWithin(T min, T max)
        {
            Extend(x =>
            {
                dynamic d = x;
                return AssertResult.New(d < min || d > max, "<name> must be within {0} and {1}", min, max);
            });
            return this;
        }

        public INumberAssertion<T> IsWithin(T min, T max)
        {
            Extend(x =>
            {
                dynamic d = x;
                return AssertResult.New(d >= min && d <= max, "<name> must not be within {0} and {1}", min, max);
            });

            return this;
        }

        public INumberAssertion<T> IsSmallerThan(T comparedTo)
        {
            Extend(x => AssertResult.New(x.CompareTo(comparedTo) <= 0, "<name> must be larger than " + comparedTo));

            return this;
        }

        public INumberAssertion<T> IsLargerThan(T comparedTo)
        {
            Extend(x => AssertResult.New(x.CompareTo(comparedTo) >= 0, "<name> must be smaller than " + comparedTo));
            return this;
        }

        public INumberAssertionU<T> Or(T obj, string withName = null)
        {
            AddWithOr(new NumberAssertion<T>(withName, obj, null, null, null));
            return this;
        }

        public new INumberAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse)
        {
            return (INumberAssertion<T>)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new INumberAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue)
        {
            return (INumberAssertion<T>)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new INumberAssertion<T> IsNotEqualTo(T comparedTo)
        {
            return (INumberAssertion<T>)base.IsNotEqualTo(comparedTo);
        }

        public new INumberAssertion<T> IsEqualTo(T comparedTo)
        {
            return (INumberAssertion<T>)base.IsEqualTo(comparedTo);
        }

    }
}
