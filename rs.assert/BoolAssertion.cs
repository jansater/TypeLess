using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RS.Assert
{
    public interface IFinalBoolAssertion : IAssertion {
        void ThenThrow();
        void ThenThrow<T>() where T : Exception;
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    public class BoolAssertion : Assertion<bool>, IFinalBoolAssertion
    {
        private List<BoolAssertion> _childAssertions = new List<BoolAssertion>();

        public BoolAssertion(string s, bool source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        internal new List<BoolAssertion> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public BoolAssertion StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public IFinalBoolAssertion IsTrue {
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

        public IFinalBoolAssertion IsFalse
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

    }
}
