using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RS.Assert
{
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public class EnumerableAssertion : Assertion<IEnumerable>
    {
        private List<EnumerableAssertion> _childAssertions = new List<EnumerableAssertion>();

        public EnumerableAssertion(string s, IEnumerable source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        internal new List<EnumerableAssertion> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public new EnumerableAssertion IsNull
        {
            get
            {
                return (EnumerableAssertion)(AssertExtensions.IsNull(this));
            }
        }

        public EnumerableAssertion StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public EnumerableAssertion IsEmpty 
        {
            get {
                return this.IsEmpty();
            }
        }


    }
}
