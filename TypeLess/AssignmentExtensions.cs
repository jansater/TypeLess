using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace TypeLess
{

    /// <summary>
    /// Throws arg null exception instead of arg exception just to avoid parameter name messages ... could use a custom exception though
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
#pragma warning disable 0436,1685
    public static class AssignmentExtensions
    {

        public static void AssignTo<T>(this T source, ref T target, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : class
        {

            source.If().IsNull.ThenThrow<AssignmentException>($"{typeof(T).Name} in {caller} of {Path.GetFileName(file)} at line {lineNumber} cannot be assigned a null value");
            target = source;
        }

        public static T CheckedAssignment<T>(this T source, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : class
        {

            source.If().IsNull.ThenThrow<AssignmentException>($"{typeof(T).Name} in {caller} of {Path.GetFileName(file)} at line {lineNumber} cannot be assigned a null value");
            return source;
        }
    }
}


