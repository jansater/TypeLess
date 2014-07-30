
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{

    /// <summary>
    /// Throws arg null exception instead of arg exception just to avoid parameter name messages ... could use a custom exception though
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
#pragma warning disable 0436,1685
    public static class AssertExtensions
    {
        internal static string GetTypeName(Type type)
        {
            var genericArgs = type.GenericTypeArguments;
            if (genericArgs.Any())
            {
                return String.Format(CultureInfo.InvariantCulture, "{0}<{1}>",
                    type.Name,
                    String.Join<string>(",", type.GenericTypeArguments.Select(x => GetTypeName(x)).ToArray()));
            }
            else
            {
                return type.Name;
            }
        }

        #region Numbers

        public static INumberAssertionU<byte> If(this byte source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<byte>(name ?? GetTypeName(typeof(byte)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<sbyte> If(this sbyte source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<sbyte>(name ?? GetTypeName(typeof(sbyte)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<ushort> If(this ushort source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<ushort>(name ?? GetTypeName(typeof(ushort)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<uint> If(this uint source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<uint>(name ?? GetTypeName(typeof(uint)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<ulong> If(this ulong source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<ulong>(name ?? GetTypeName(typeof(ulong)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<short> If(this short source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<short>(name ?? GetTypeName(typeof(short)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<long> If(this long source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<long>(name ?? GetTypeName(typeof(long)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<char> If(this char source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<char>(name ?? GetTypeName(typeof(char)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<int> If(this int source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<int>(name ?? GetTypeName(typeof(int)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<decimal> If(this decimal source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<decimal>(name ?? GetTypeName(typeof(decimal)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<float> If(this float source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<float>(name ?? GetTypeName(typeof(float)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static INumberAssertionU<double> If(this double source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new NumberAssertion<double>(name ?? GetTypeName(typeof(double)), source, Path.GetFileName(file), lineNumber, caller);
        }


        #endregion

        public static INullableAssertionU<T> If<T>(this Nullable<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : struct
        {
            
            return new NullableAssertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static ITimeSpanAssertionU If(this TimeSpan source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new TimeSpanAssertion(name ?? GetTypeName(typeof(TimeSpan)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IDateTimeAssertionU If(this DateTime source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new DateTimeAssertion(name ?? GetTypeName(typeof(DateTime)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IBoolAssertionU If(this bool source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new BoolAssertion(name ?? GetTypeName(typeof(bool)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IDictionaryAssertionU<T, T2> If<T, T2>(this IDictionary<T, T2> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new DictionaryAssertion<T, T2>(name ?? GetTypeName(typeof(IDictionary<T, T2>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IDictionaryAssertionU<T, T2> If<T, T2>(this Dictionary<T, T2> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new DictionaryAssertion<T, T2>(name ?? GetTypeName(typeof(Dictionary<T, T2>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If<T>(this T[] source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(T[])), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If<T>(this Stack<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(Stack<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If<T>(this Queue<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(Queue<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If<T>(this IList<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IList<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If<T>(this List<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(List<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If(this IEnumerable source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If<T>(this IEnumerable<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IStringAssertionU If(this string source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            
            return new StringAssertion(name ?? GetTypeName(typeof(string)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IClassAssertionU<T> If<T>(this T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : class
        {
            
            return new ClassAssertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        internal static Assertion<T> CreateAssert<T>(this T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) //where T : class
        {
            
            return new Assertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        internal static IAssertion<T> IsTrue<T>(this Assertion<T> source, Func<T, bool> assertFunc, string msgIfFalse = null)
        {
            if (assertFunc == null)
            {
                throw new ArgumentNullException("assertFunc is null");
            }

            
            source.Extend(x =>
            {
                if (msgIfFalse == null)
                {
                    return AssertResult.New(assertFunc(source.Item), msgIfFalse);
                }
                else {
                    msgIfFalse = msgIfFalse.Replace("<name>", "{0}");
                    return AssertResult.New(assertFunc(source.Item),
                        String.Format(CultureInfo.InvariantCulture, msgIfFalse, source.Name));
                }
                
            });
            return source;
        }

        internal static IAssertion<T> IsFalse<T>(this Assertion<T> source, Func<T, bool> assertFunc, string msgIfTrue)
        {
            if (assertFunc == null)
            {
                throw new ArgumentNullException("assertFunc is null");
            }

            

            source.Extend(x =>
            {
                if (msgIfTrue == null)
                {
                    return AssertResult.New(!assertFunc(x), msgIfTrue);
                }
                else {
                    msgIfTrue = msgIfTrue.Replace("<name>", "{0}");
                    return AssertResult.New(!assertFunc(x), String.Format(CultureInfo.InvariantCulture, msgIfTrue, source.Name));
                }
                
            });
            return source;
        }

        internal static IAssertion<T> IsNotEqualTo<T>(this Assertion<T> source, T comparedTo){
            source.Extend(x =>
            {
                if (x == null)
                {
                    return AssertResult.New(comparedTo != null, Resources.IsNotEqualTo, comparedTo);
                }

                return AssertResult.New(!x.Equals(comparedTo), Resources.IsNotEqualTo, comparedTo == null ? "null" : comparedTo.ToString());
            });
            return source;
        }

        internal static IAssertion<T> IsEqualTo<T>(this Assertion<T> source, T comparedTo)
        {
            source.Extend(x =>
            {
                if (x == null)
                {
                    return AssertResult.New(comparedTo == null, Resources.IsEqualTo, comparedTo);
                }

                return AssertResult.New(x.Equals(comparedTo), Resources.IsEqualTo, comparedTo == null ? "null" : comparedTo.ToString());
            });
            return source;
        }

    }
}


