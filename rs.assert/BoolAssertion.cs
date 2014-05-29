using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TypeLess
{
    public interface IBoolAssertionU : IAssertion<bool>
    {
        new IBoolAssertion IsTrue { get; }
        new IBoolAssertion IsFalse { get; }
        IBoolAssertion Or(bool obj, string withName = null);
        new IBoolAssertion IsNotEqualTo(bool comparedTo);
        new IBoolAssertion IsEqualTo(bool comparedTo);
    }

    public interface IBoolAssertion : IBoolAssertionU, ICompleteAssertion {
        
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

        public IBoolAssertion StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new IBoolAssertion IsTrue {
            get {
                
                if (Item)
                {
                    Append("must be false");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsTrue);
                }

                return this;
            }
        }

        public new IBoolAssertion IsFalse
        {
            get
            {

                if (!Item)
                {
                    Append("must be true");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsTrue);
                }

                return this;
            }
        }

        public IBoolAssertion Or(bool obj, string withName = null)
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
