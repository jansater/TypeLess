using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TypeLess
{
    public interface IEnumerableAssertionU : IAssertionU<IEnumerable> {
        IEnumerableAssertion IsNull { get; }
        IEnumerableAssertion IsEmpty { get; }
        IEnumerableAssertion ContainsLessThan(int nElements);
        IEnumerableAssertion ContainsMoreThan(int nElements);
        
        IEnumerableAssertionU Or(IEnumerable obj, string withName = null);

        new IEnumerableAssertion IsTrue(Func<IEnumerable, bool> assertFunc, string errMsg = null);
        new IEnumerableAssertion IsFalse(Func<IEnumerable, bool> assertFunc, string errMsg = null);

        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        new IEnumerableAssertion EvalPositive { get; }
    }

    public interface IEnumerableAssertion : IEnumerableAssertionU, IAssertion<IEnumerable>
    { 
        
    }

    

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class EnumerableAssertion : ClassAssertion<IEnumerable>, IEnumerableAssertion
    {
        private List<EnumerableAssertion> _childAssertions = new List<EnumerableAssertion>();

        public EnumerableAssertion(string s, IEnumerable source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public IEnumerableAssertion Combine(IEnumerableAssertion otherAssertion)
        {
            return (IEnumerableAssertion)base.Or(otherAssertion);
        }

        public new IEnumerableAssertion StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new IEnumerableAssertion IsNull
        {
            get
            {
                var x = base.IsNull;
                return this;
            }
        }

        public IEnumerableAssertion IsEmpty 
        {
            get {
                return this.IsEmpty();
            }
        }

        public IEnumerableAssertion ContainsLessThan(int nElements)
        {
            return EnumerableAssertExtensions.ContainsLessThan(this, nElements);
        }

        public IEnumerableAssertion ContainsMoreThan(int nElements)
        {
            return EnumerableAssertExtensions.ContainsMoreThan(this, nElements);
        }

        public new IEnumerableAssertionU Or(IEnumerable obj, string withName = null)
        {
            AddWithOr(new EnumerableAssertion(withName, obj, null, null, null));
            return this;
        }

        public new IEnumerableAssertion IsTrue(Func<IEnumerable, bool> assertFunc, string errMsg = null)
        {
            return (IEnumerableAssertion)base.IsTrue(assertFunc, errMsg);
        }

        public new IEnumerableAssertion IsFalse(Func<IEnumerable, bool> assertFunc, string errMsg = null)
        {
            return (IEnumerableAssertion)base.IsFalse(assertFunc, errMsg);
        }

        public new IEnumerableAssertion IsNotEqualTo(IEnumerable comparedTo)
        {
            return (IEnumerableAssertion)base.IsNotEqualTo(comparedTo);
        }

        public new IEnumerableAssertion IsEqualTo(IEnumerable comparedTo)
        {
            return (IEnumerableAssertion)base.IsEqualTo(comparedTo);
        }



        public new IEnumerableAssertion EvalPositive
        {
            get { return (IEnumerableAssertion)base.EvalPositive; }
        }
    }
}
