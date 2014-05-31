using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TypeLess
{
    public interface IBoolAssertionU : IAssertionU<bool>
    {
        new IBoolAssertion IsTrue { get; }
        new IBoolAssertion IsFalse { get; }
        IBoolAssertionU Or(bool obj, string withName = null);
        new IBoolAssertion IsNotEqualTo(bool comparedTo);
        new IBoolAssertion IsEqualTo(bool comparedTo);
        
    }

    public interface IBoolAssertion : IBoolAssertionU, IAssertion<bool> {
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class BoolAssertion : Assertion<bool>, IBoolAssertion
    {
        private List<BoolAssertion> _childAssertions = new List<BoolAssertion>();

        public BoolAssertion(string s, bool source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public IBoolAssertion Combine(IBoolAssertion otherAssertion)
        {
            return (IBoolAssertion)base.Combine(otherAssertion);
        }

        internal new List<BoolAssertion> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public new IBoolAssertion StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new IBoolAssertion IsTrue {
            get {

                Extend(x => x ? "must be false" : null, x => this);
                return this;
            }
        }

        public new IBoolAssertion IsFalse
        {
            get
            {
                Extend(x => !x ? "must be true" : null, x => this);
                return this;
            }
        }

        public IBoolAssertionU Or(bool obj, string withName = null)
        {
            this.ChildAssertions.Add(new BoolAssertion(withName, obj, null, null, null));
            return this;
        }

        public new IBoolAssertion IsNotEqualTo(bool comparedTo)
        {
            return (IBoolAssertion)base.IsNotEqualTo(comparedTo);
        }

        public new IBoolAssertion IsEqualTo(bool comparedTo)
        {
            return (IBoolAssertion)base.IsEqualTo(comparedTo);
        }

    }
}
