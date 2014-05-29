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
        IEnumerableAssertion Or(IEnumerable obj, string withName = null);

        new IEnumerableAssertion IsTrue(Func<IEnumerable, bool> assertFunc, string msgIfFalse);
        new IEnumerableAssertion IsFalse(Func<IEnumerable, bool> assertFunc, string msgIfTrue);
        new IEnumerableAssertion IsNotEqualTo(IEnumerable comparedTo);
        new IEnumerableAssertion IsEqualTo(IEnumerable comparedTo);
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
            return (IEnumerableAssertion)base.Combine(otherAssertion);
        }

        internal new List<EnumerableAssertion> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
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

        public new IEnumerableAssertion Or(IEnumerable obj, string withName = null)
        {
            this.ChildAssertions.Add(new EnumerableAssertion(withName, obj, null, null, null));
            return this;
        }

        public new IEnumerableAssertion IsTrue(Func<IEnumerable, bool> assertFunc, string msgIfFalse)
        {
            return (IEnumerableAssertion)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new IEnumerableAssertion IsFalse(Func<IEnumerable, bool> assertFunc, string msgIfTrue)
        {
            return (IEnumerableAssertion)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new IEnumerableAssertion IsNotEqualTo(IEnumerable comparedTo)
        {
            return (IEnumerableAssertion)base.IsNotEqualTo(comparedTo);
        }

        public new IEnumerableAssertion IsEqualTo(IEnumerable comparedTo)
        {
            return (IEnumerableAssertion)base.IsEqualTo(comparedTo);
        }

    }
}
