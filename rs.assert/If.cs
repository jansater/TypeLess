using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RS.Assert
{

    public class AnyAssertion<T> : Assertion<T> {
        public AnyAssertion(string name, T source, string file, int? lineNumber, string caller)
            : base (name, source, file, lineNumber, caller) {}

    }

    public static class If
    {
        
        public static Assertion<T> AnyOf<T>(T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) {

            return new AnyAssertion<T>(name ?? AssertExtensions.GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }
    }
}
