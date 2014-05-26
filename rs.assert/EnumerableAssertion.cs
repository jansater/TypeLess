using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace RS.Assert
{
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public class EnumerableAssertion : Assertion<IEnumerable>
    {
        public EnumerableAssertion(string s, IEnumerable source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

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
