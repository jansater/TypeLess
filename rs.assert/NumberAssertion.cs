using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace TypeLess
{

    public interface INumberAssertionU<T> : IAssertionU<T> where T : struct, IComparable<T>
    {
        INumberAssertion<T> IsZero { get; }
        INumberAssertion<T> IsPositive { get; }
        INumberAssertion<T> IsNegative { get; }
        INumberAssertion<T> IsNotWithin(T min, T max);
        INumberAssertion<T> IsWithin(T min, T max);
        INumberAssertion<T> IsSmallerThan(T comparedTo);
        INumberAssertion<T> IsLargerThan(T comparedTo);
        INumberAssertion<T> Or(T obj, string withName = null);

        new INumberAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse);
        new INumberAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue);
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

        internal new List<NumberAssertion<T>> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

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
                if (this.IgnoreFurtherChecks)
                {
                    return this;
                }

                dynamic d = this.Item;
                if (d == 0)
                {
                    Append("must be non zero");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsZero);
                }

                return this;
            }

        }

        public INumberAssertion<T> IsPositive
        {
            get
            {
                if (IgnoreFurtherChecks)
                {
                    return this;
                }

                dynamic d = Item;

                if (d > 0.0)
                {
                    Append("must be zero or negative");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsPositive);
                }

                return this;
            }

        }

        public INumberAssertion<T> IsNegative
        {
            get
            {
                if (IgnoreFurtherChecks)
                {
                    return this;
                }

                dynamic d = Item;
                if (d < 0.0)
                {
                    Append("must be zero or positive");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsNegative);
                }

                return this;
            }

        }

        public INumberAssertion<T> IsNotWithin(T min, T max)
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

        public INumberAssertion<T> IsWithin(T min, T max)
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

        public INumberAssertion<T> IsSmallerThan(T comparedTo)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if (Item.CompareTo(comparedTo) <= 0)
            {
                Append("must be larger than " + comparedTo);
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsSmallerThan(comparedTo));
            }

            return this;
        }

        public INumberAssertion<T> IsLargerThan(T comparedTo)
        {
            if (IgnoreFurtherChecks)
            {
                return this;
            }

            if (Item.CompareTo(comparedTo) >= 0)
            {
                Append("must be smaller than " + comparedTo);
            }

            foreach (var child in ChildAssertions)
            {
                child.ClearErrorMsg();
                Combine(child.IsLargerThan(comparedTo));
            }

            return this;
        }


        public INumberAssertion<T> Or(T obj, string withName = null)
        {
            this.ChildAssertions.Add(new NumberAssertion<T>(withName, obj, null, null, null));
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
