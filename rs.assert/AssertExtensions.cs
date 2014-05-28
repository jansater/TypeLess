
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

        public static Assertion<T> And<T, S>(this Assertion<T> source, S obj, string withName = null)
        {
            source.Add(obj, withName);
            return source;
        }

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

        public static Assertion<T> StopIfNotValid<T>(this Assertion<T> source)
        {
            source.IgnoreFurtherChecks = true;
            return source;
        }

        public static BoolAssertion If(this bool source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new BoolAssertion(name ?? GetTypeName(typeof(bool)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static EnumerableAssertion If<T>(this List<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static EnumerableAssertion If(this IEnumerable source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static EnumerableAssertion If<T>(this IEnumerable<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static StringAssertion If(this string source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new StringAssertion(name ?? GetTypeName(typeof(string)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static Assertion<T> If<T>(this T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            //name = UpdateName(name, file, lineNumber);
            return new Assertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        /// <summary>
        /// Determines whether the specified source is null. Automatically stops further processing if source is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        internal static Assertion<T> IsNull<T>(this Assertion<T> source)
        {

            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (source.Item == null)
            {
                source.StopIfNotValid();
                source.Append("is required");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.IsNull);
            }

            return source;
        }

        public static Assertion<T> IsTrue<T>(this Assertion<T> source, Func<T, bool> assertFunc, string msgIfFalse)
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

        public static Assertion<T> IsFalse<T>(this Assertion<T> source, Func<T, bool> assertFunc, string msgIfTrue)
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

        public static Assertion<T> IsZero<T>(this Assertion<T> source) where T : struct, 
          IComparable,
          IComparable<T>,
          IEquatable<T>,
          IFormattable
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            dynamic d = source.Item;
            if (d == 0)
            {
                source.Append("must be non zero");
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsZero());
            }

            return source;
        }

        public static Assertion<T> IsNotEqualTo<T>(this Assertion<T> source, T comparedTo) where T : IComparable<T>
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
                        source.Append("must not be equal to " + comparedTo.ToString());

                    }
                    return source;
                }

                if (source.Item.CompareTo(comparedTo) != 0)
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
                    source.Combine(c.IsNotEqualTo(comparedTo));
                }
            }

        }

        public static Assertion<T> IsEqualTo<T>(this Assertion<T> source, T comparedTo) where T : IComparable<T>
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
                        source.Append("must be equal to " + comparedTo.ToString());

                    }
                    return source;
                }

                if (source.Item.CompareTo(comparedTo) == 0)
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
                    source.Combine(c.IsEqualTo(comparedTo));
                }
            }
        }

        public static Assertion<T> IsSmallerThan<T>(this Assertion<T> source, T comparedTo) where T : IComparable<T>
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (source.Item.CompareTo(comparedTo) <= 0)
            {
                source.Append("must be larger than " + comparedTo);
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsSmallerThan(comparedTo));
            }

            return source;
        }

        public static Assertion<T> IsLargerThan<T>(this Assertion<T> source, T comparedTo) where T : IComparable<T>
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (source.Item.CompareTo(comparedTo) >= 0)
            {
                source.Append("must be smaller than " + comparedTo);
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsLargerThan(comparedTo));
            }

            return source;
        }

        public static Assertion<T> IsPositive<T>(this Assertion<T> source) where T : struct, 
          IComparable,
          IComparable<T>,
          IEquatable<T>,
          IFormattable
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            dynamic d = source.Item;

            if (d > 0.0)
            {
                source.Append("must be zero or negative");
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsPositive());
            }

            return source;
        }

        public static Assertion<T> IsNegative<T>(this Assertion<T> source) where T : struct, 
          IComparable,
          IComparable<T>,
          IEquatable<T>,
          IFormattable
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            dynamic d = source.Item;
            if (d < 0.0)
            {
                source.Append("must be zero or positive");
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsNegative());
            }

            return source;
        }

        public static Assertion<T> IsNotWithin<T>(this Assertion<T> source, T min, T max) where T : struct, 
          IComparable,
          IComparable<T>,
          IEquatable<T>,
          IFormattable
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            dynamic d = source.Item;
            if (d < min || d > max)
            {
                source.Append(String.Format("must be within {0} and {1}", min, max));
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsNotWithin(min, max));
            }

            return source;
        }

        /// <summary>
        /// Make a call to this class IsValid method to determine whether the specified target object is valid. Normally used to define validation checks in for example dto's. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The target object.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        internal static Assertion<T> IsInvalid<T>(this Assertion<T> source)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            source = source.IsNull();

            if (source.Item != null)
            {
                dynamic d = source.Item;
                try
                {
                    var classAssertions = d.IsInvalid() as IEnumerable<IAssertion>;
                    foreach (var item in classAssertions)
                    {
                        source = source.Combine(item);
                    }
                }
                catch (RuntimeBinderException)
                {
                    throw new System.MissingMemberException("You must define method public IEnumerable<IAssertion> IsInvalid() {} in class " + typeof(T).Name);
                }
            }

            foreach (var child in source.ChildAssertions)
            {
                var c = child.Cast<T>();
                c.ClearErrorMsg();
                source.Combine(c.IsInvalid());
            }

            return source;
        }

    }
}


