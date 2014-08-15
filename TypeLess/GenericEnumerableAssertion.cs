using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{

    public interface IEnumerableAssertionU<T> : IAssertionU<IEnumerable<T>>
    {
        IEnumerableAssertion<T> IsNull { get; }
        IEnumerableAssertion<T> IsEmpty { get; }
        IEnumerableAssertion<T> ContainsLessThan(int nElements);
        IEnumerableAssertion<T> ContainsMoreThan(int nElements);

        IEnumerableAssertion<T> Contains(params T[] item);
        IEnumerableAssertion<T> DoesNotContain(params T[] item);

        IEnumerableAssertionU<T> Or(IEnumerable<T> obj, string withName = null);

        new IEnumerableAssertion<T> IsTrue(Func<IEnumerable<T>, bool> assertFunc, string errMsg = null);
        new IEnumerableAssertion<T> IsFalse(Func<IEnumerable<T>, bool> assertFunc, string errMsg = null);

        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        new IEnumerableAssertion<T> EvalPositive { get; }
    }

    public interface IEnumerableAssertion<T> : IEnumerableAssertionU<T>, IAssertion<IEnumerable<T>>
    {

    }


#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class GenericEnumerableAssertion<T> : ClassAssertion<IEnumerable<T>>,
        IEnumerableAssertionU<T>, IEnumerableAssertion<T>
    {
        private List<GenericEnumerableAssertion<T>> _childAssertions = new List<GenericEnumerableAssertion<T>>();

        public GenericEnumerableAssertion(string s, IEnumerable<T> source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public IEnumerableAssertion<T> Combine(IEnumerableAssertion<T> otherAssertion)
        {
            return (IEnumerableAssertion<T>)base.Or(otherAssertion);
        }

        public new IEnumerableAssertion<T> StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new IEnumerableAssertion<T> IsNull
        {
            get
            {
                var x = base.IsNull;
                return this;
            }
        }

        public IEnumerableAssertion<T> IsEmpty 
        {
            get {
                Extend(x =>
                {
                    var c = x.Count();
                    return AssertResult.New(c == 0, Resources.IsEmpty);
                });
                return this;
            }
        }

        public IEnumerableAssertion<T> ContainsLessThan(int nElements)
        {
            Extend(x =>
            {
                var count = x.Count();
                return AssertResult.New(count < nElements, Resources.ContainsLessThan, nElements);
            });
            return this;
        }

        public IEnumerableAssertion<T> ContainsMoreThan(int nElements)
        {
            Extend(x =>
            {
                var count = x.Count();
                return AssertResult.New(count > nElements, Resources.ContainsMoreThan, nElements);
            });
            return this;
        }

        public new IEnumerableAssertionU<T> Or(IEnumerable<T> obj, string withName = null)
        {
            AddWithOr(new GenericEnumerableAssertion<T>(withName, obj, null, null, null));
            return this;
        }

        public new IEnumerableAssertion<T> IsTrue(Func<IEnumerable<T>, bool> assertFunc, string errMsg = null)
        {
            return (IEnumerableAssertion<T>)base.IsTrue(assertFunc, errMsg);
        }

        public new IEnumerableAssertion<T> IsFalse(Func<IEnumerable<T>, bool> assertFunc, string errMsg = null)
        {
            return (IEnumerableAssertion<T>)base.IsFalse(assertFunc, errMsg);
        }

        public new IEnumerableAssertion<T> IsNotEqualTo(IEnumerable<T> comparedTo)
        {
            return (IEnumerableAssertion<T>)base.IsNotEqualTo(comparedTo);
        }

        public new IEnumerableAssertion<T> IsEqualTo(IEnumerable<T> comparedTo)
        {
            return (IEnumerableAssertion<T>)base.IsEqualTo(comparedTo);
        }



        public new IEnumerableAssertion<T> EvalPositive
        {
            get { return (IEnumerableAssertion<T>)base.EvalPositive; }
        }


        public IEnumerableAssertion<T> Contains(params T[] items)
        {
            Extend(x =>
            {
                var containsItems = items.All(y => x.Contains(y));

                return AssertResult.New(containsItems, Resources.ContainsItems, String.Join(",", items.Select(y => y.ToString()).ToArray()));
            });
            return this;
        }

        public IEnumerableAssertion<T> DoesNotContain(params T[] items)
        {
            Extend(x =>
            {
                var containsItems = items.Any(y => !x.Contains(y));

                return AssertResult.New(containsItems, Resources.DoesNotContainItems, String.Join(",", items.Select(y => y.ToString()).ToArray()));
            });
           
            return this;
        }
    }
}
