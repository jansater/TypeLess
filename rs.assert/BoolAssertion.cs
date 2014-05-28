using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RS.Assert
{
    public interface IBoolAssertion : IAssertion<bool> {
        new IBoolAssertion IsTrue { get; }
        new IBoolAssertion IsFalse { get; }
        IBoolAssertion Or(bool obj, string withName = null);
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

        public IBoolAssertion IsTrue {
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

        public IBoolAssertion IsFalse
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

        public new IBoolAssertion Or(bool obj, string withName = null)
        {
            this.ChildAssertions.Add(new BoolAssertion(withName, obj, null, null, null));
            return this;
        }

    }
}
