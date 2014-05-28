
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace RS.Assert
{

    /// <summary>
    /// Throws arg null exception instead of arg exception just to avoid parameter name messages ... could use a custom exception though
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public static class AssertExtensions
    {

        //private static string TryReadLine(string filePath, int line) {
        //    try
        //    {
        //        using (Stream stream = File.Open(filePath, FileMode.Open))
        //        {
        //            using (StreamReader reader = new StreamReader(stream))
        //            {
        //                string row = null;
        //                for (int i = 0; i < line && !reader.EndOfStream; i++)
        //                {
        //                    row = reader.ReadLine();
        //                }

        //                return row;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }

        //}

        internal static string GetTypeName(Type type)
        {
            var genericArgs = type.GenericTypeArguments;
            if (genericArgs.Any())
            {
                return String.Format("{0}<{1}>",
                    type.Name,
                    String.Join<string>(",", type.GenericTypeArguments.Select(x => GetTypeName(x)).ToArray()));
            }
            else
            {
                return type.Name;
            }
        }

        //private static string UpdateName(string name, string file, int? lineNumber) {

        //    if (String.IsNullOrEmpty(name) && Debugger.IsAttached && file != null && lineNumber.HasValue) {
        //        var line = TryReadLine(file, lineNumber.Value);
        //        if (line != null) {
        //            var match = Regex.Match(line, "(?<caller>\\S*?).If\\(");
        //            if (match.Success) {
        //                var callingProperty = match.Result("$1");
        //                if (!String.IsNullOrWhiteSpace(callingProperty)) {
        //                    name = callingProperty;
        //                }
        //            }
        //        }
        //    }

        //    return name;

        //}


        #region Numbers

        public static INumberAssertion<byte> If(this byte source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<byte>(name ?? GetTypeName(typeof(byte)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<sbyte> If(this sbyte source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<sbyte>(name ?? GetTypeName(typeof(sbyte)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<ushort> If(this ushort source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<ushort>(name ?? GetTypeName(typeof(ushort)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<uint> If(this uint source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<uint>(name ?? GetTypeName(typeof(uint)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<ulong> If(this ulong source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<ulong>(name ?? GetTypeName(typeof(ulong)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<short> If(this short source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<short>(name ?? GetTypeName(typeof(short)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<long> If(this long source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<long>(name ?? GetTypeName(typeof(long)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<char> If(this char source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<char>(name ?? GetTypeName(typeof(char)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<int> If(this int source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<int>(name ?? GetTypeName(typeof(int)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<decimal> If(this decimal source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<decimal>(name ?? GetTypeName(typeof(decimal)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<float> If(this float source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<float>(name ?? GetTypeName(typeof(float)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertion<double> If(this double source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new NumberAssertion<double>(name ?? GetTypeName(typeof(double)), source, Path.GetFileName(file), lineNumber, caller);
        }


        #endregion

        public static INullableAssertion<T> If<T>(this Nullable<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : struct
        {
            //name = UpdateName(name, file, lineNumber);
            return new NullableAssertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static ITimeSpanAssertion If(this TimeSpan source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new TimeSpanAssertion(name ?? GetTypeName(typeof(TimeSpan)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IDateTimeAssertion If(this DateTime source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new DateTimeAssertion(name ?? GetTypeName(typeof(DateTime)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IBoolAssertion If(this bool source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new BoolAssertion(name ?? GetTypeName(typeof(bool)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertion If<T>(this List<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertion If(this IEnumerable source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertion If<T>(this IEnumerable<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IStringAssertion If(this string source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new StringAssertion(name ?? GetTypeName(typeof(string)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IClassAssertion<T> If<T>(this T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : class
        {
            //name = UpdateName(name, file, lineNumber);
            return new ClassAssertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        internal static Assertion<T> CreateAssert<T>(this T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : class
        {
            //name = UpdateName(name, file, lineNumber);
            return new Assertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        internal static IAssertion<T> IsTrue<T>(this Assertion<T> source, Func<T, bool> assertFunc, string msgIfFalse)
        {
            if (assertFunc == null)
            {
                throw new ArgumentNullException("assertFunc is null");
            }

            if (msgIfFalse == null)
            {
                throw new ArgumentNullException("msgIfFalse");
            }

            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (assertFunc(source.Item))
            {
                source.Append(msgIfFalse);
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsTrue(assertFunc, msgIfFalse.Replace(source.Name, c.Name)));
            }

            return source;
        }

        internal static IAssertion<T> IsFalse<T>(this Assertion<T> source, Func<T, bool> assertFunc, string msgIfTrue)
        {
            if (assertFunc == null)
            {
                throw new ArgumentNullException("assertFunc is null");
            }

            if (msgIfTrue == null)
            {
                throw new ArgumentNullException("msgIfTrue");
            }

            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (!assertFunc(source.Item))
            {
                source.Append(msgIfTrue);
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsFalse(assertFunc, msgIfTrue.Replace(source.Name, c.Name)));
            }

            return source;
        }

        internal static IAssertion<T> IsNotEqualTo<T>(this Assertion<T> source, T comparedTo) 
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            try
            {
                if (source.Item == null)
                {
                    if (comparedTo != null)
                    {
                        source.Append("must be equal to " + comparedTo.ToString());

                    }
                    return source;
                }

                if (!source.Item.Equals(comparedTo))
                {
                    source.Append(string.Format("must be equal to {0}", comparedTo == null ? "null" : comparedTo.ToString()));
                }
                return source;
            }
            finally
            {
                foreach (var child in source.ChildAssertions)
                {
                    var c = child.Cast<T>();
                    c.ClearErrorMsg();
                    source.Combine(c.IsNotEqualTo(comparedTo));
                }
            }

        }

        internal static IAssertion<T> IsEqualTo<T>(this Assertion<T> source, T comparedTo)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            try
            {
                if (source.Item == null)
                {
                    if (comparedTo == null)
                    {
                        source.Append("must not be equal to " + comparedTo.ToString());

                    }
                    return source;
                }

                if (source.Item.Equals(comparedTo))
                {
                    source.Append(string.Format("must not be equal to {0}", comparedTo == null ? "null" : comparedTo.ToString()));
                }
                return source;
            }
            finally
            {
                foreach (var child in source.ChildAssertions)
                {
                    var c = child.Cast<T>();
                    c.ClearErrorMsg();
                    source.Combine(c.IsEqualTo(comparedTo));
                }
            }
        }


    }
}


