using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using TypeLess.DataTypes;
using TypeLess.Helpers;
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
        
        public static bool In<T>(this T val, params T[] values) where T : struct
        {
            return values.Contains(val);
        }

        public static bool NotIn<T>(this T val, params T[] values) where T : struct
        {
            return !values.Contains(val);
        }

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

        public static T ShallowCopy<T>(this T obj) where T : class {

            var t = typeof(T);
            var newInstance = ObjectFactory.CreateObject<T>();
            ObjectFactory.ShallowCopyProperties(obj, newInstance);
            return newInstance;
        }

        #region QuickAsserts

        public static IAssertionOW<bool> ThenThrow<E>(this bool b, string errorMsg, params object[] args) where E : Exception
        {
            if (!b)
            {
                return new Assertion<bool>(errorMsg, b, null, null, null);
            }

            if (errorMsg != null && args != null && args.Length > 0)
            {
                errorMsg = String.Format(CultureInfo.InvariantCulture, errorMsg, args);
            }

            throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg });
        }
        
        public static T ThenReturn<T>(this bool b, Func<T> action) {
            var assertion = b.If().IsTrue;
            return assertion.ThenReturn(action);
        }

        public static IAssertionOW<bool> Then(this bool b, Action<bool> action)
        {
            var assertion = b.If().IsTrue;
            return assertion.Then(action);
        }
        
        public static IAssertionOW<bool> ThenThrow<E>(this bool b, Exception innerException, string errorMsg, params object[] args) where E : Exception
        {
            if (!b)
            {
                return new Assertion<bool>(errorMsg, b, null, null, null);
            }

            if (errorMsg != null && args != null && args.Length > 0)
            {
                errorMsg = String.Format(CultureInfo.InvariantCulture, errorMsg, args);
            }

            throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg, innerException });
        }

        #endregion

        #region IfAsserts

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

        public static IEnumerableAssertionU<T> If<T>(this T[] source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new GenericEnumerableAssertion<T>(name ?? GetTypeName(typeof(T[])), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU<T> If<T>(this Stack<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new GenericEnumerableAssertion<T>(name ?? GetTypeName(typeof(Stack<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU<T> If<T>(this Queue<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new GenericEnumerableAssertion<T>(name ?? GetTypeName(typeof(Queue<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU<T> If<T>(this IList<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new GenericEnumerableAssertion<T>(name ?? GetTypeName(typeof(IList<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU<T> If<T>(this List<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new GenericEnumerableAssertion<T>(name ?? GetTypeName(typeof(List<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU If(this IEnumerable source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new EnumerableAssertion(name ?? GetTypeName(typeof(IEnumerable)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerableAssertionU<T> If<T>(this IEnumerable<T> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new GenericEnumerableAssertion<T>(name ?? GetTypeName(typeof(IEnumerable<T>)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IStringAssertionU If(this string source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            return new StringAssertion(name ?? GetTypeName(typeof(string)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IClassAssertionU<T> If<T>(this T source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where T : class
        {

            return new ClassAssertion<T>(name ?? GetTypeName(typeof(T)), source, Path.GetFileName(file), lineNumber, caller);
        }

        public static IEnumerable<Difference> DiffObjects<T>(this T source, object target, bool throwExceptionOnPropertyMismatch = false, string[] ignoreProperties = null) where T : class
        {
            return (source.If() as ClassAssertion<T>).DiffProperties(source, target, throwExceptionOnPropertyMismatch: throwExceptionOnPropertyMismatch, ignoreProperties: ignoreProperties);
        }

        #endregion

        #region AndAsserts 

        public static IEnumerableAssertionU<S> And<T, S>(this IAssertion<T> assert, S[] source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, IEnumerable<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<byte> And<T>(this IAssertion<T> assert, byte source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, byte>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<sbyte> And<T>(this IAssertion<T> assert, sbyte source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, sbyte>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<ushort> And<T>(this IAssertion<T> assert, ushort source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, ushort>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<uint> And<T>(this IAssertion<T> assert, uint source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, uint>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<ulong> And<T>(this IAssertion<T> assert, ulong source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, ulong>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<short> And<T>(this IAssertion<T> assert, short source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, short>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<long> And<T>(this IAssertion<T> assert, long source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, long>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<char> And<T>(this IAssertion<T> assert, char source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, char>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<int> And<T>(this IAssertion<T> assert, int source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, int>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<decimal> And<T>(this IAssertion<T> assert, decimal source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, decimal>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<float> And<T>(this IAssertion<T> assert, float source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, float>(assert, nextAssert);
            return nextAssert;
        }

        public static INumberAssertionU<double> And<T, S>(this IAssertion<T> assert, double source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, double>(assert, nextAssert);
            return nextAssert;
        }

        public static INullableAssertionU<S> And<T, S>(this IAssertion<T> assert, Nullable<S> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where S : struct
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, Nullable<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static ITimeSpanAssertionU And<T>(this IAssertion<T> assert, TimeSpan source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, TimeSpan>(assert, nextAssert);
            return nextAssert;
        }

        public static IDateTimeAssertionU And<T>(this IAssertion<T> assert, DateTime source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, DateTime>(assert, nextAssert);
            return nextAssert;
        }

        public static IBoolAssertionU And<T, S>(this IAssertion<T> assert, bool source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, bool>(assert, nextAssert);
            return nextAssert;
        }

        public static IDictionaryAssertionU<S, S2> And<T, S, S2>(this IAssertion<T> assert, IDictionary<S, S2> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {

            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, IDictionary<S, S2>>(assert, nextAssert);
            return nextAssert;
        }

        public static IDictionaryAssertionU<S, S2> And<T, S, S2>(this IAssertion<T> assert, Dictionary<S, S2> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, Dictionary<S, S2>>(assert, nextAssert);
            return nextAssert;
        }

        public static IEnumerableAssertionU<S> And<T, S>(this IAssertion<T> assert, Stack<S> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, Stack<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static IEnumerableAssertionU<S> And<T, S>(this IAssertion<T> assert, Queue<S> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, Queue<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static IEnumerableAssertionU<S> And<T, S>(this IAssertion<T> assert, IList<S> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, IList<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static IEnumerableAssertionU<S> And<T, S>(this IAssertion<T> assert, List<S> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, List<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static IEnumerableAssertionU And<T>(this IAssertion<T> assert, IEnumerable source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, IEnumerable>(assert, nextAssert);
            return nextAssert;
        }

        public static IEnumerableAssertionU<S> And<T, S>(this IAssertion<T> assert, IEnumerable<S> source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, IEnumerable<S>>(assert, nextAssert);
            return nextAssert;
        }

        public static IStringAssertionU And<T>(this IAssertion<T> assert, string source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null)
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, string>(assert, nextAssert);
            return nextAssert;
        }

        public static IClassAssertionU<S> And<T, S>(this IAssertion<T> assert, S source, string name = null, [CallerFilePath] string file = null, [CallerLineNumber] int? lineNumber = null, [CallerMemberName] string caller = null) where S : class
        {
            var nextAssert = source.If(name, file, lineNumber, caller);
            InternalAndOperation<T, S>(assert, nextAssert);
            return nextAssert;
        }

        #endregion

        private static void InternalAndOperation<T, S>(IAssertion assert, IAssertionU nextAssert)
        {
            var assertObj = assert as Assertion<T>;
            if (assertObj == null)
            {
                throw new InvalidOperationException("And operations are not allowed on this type of assertion");
            }

            var nextAssertObj = nextAssert as Assertion<S>;

            if (nextAssertObj == null)
            {
                throw new InvalidOperationException("And operations are not allowed on this type of assertion");
            }

            nextAssertObj.MakePartOfAndOperation(assertObj.ToString(skipTrace: true));
        }

        internal static IAssertion<T> IsTrue<T>(this Assertion<T> source, Func<T, bool> assertFunc, string errMsg = null)
        {
            if (assertFunc == null)
            {
                throw new ArgumentNullException("assertFunc is null");
            }

            
            source.Extend(x =>
            {
                if (errMsg == null)
                {
                    return AssertResult.New(assertFunc(source.Item), errMsg);
                }
                else {
                    errMsg = errMsg.Replace("<name>", "{0}");
                    return AssertResult.New(assertFunc(source.Item),
                        String.Format(CultureInfo.InvariantCulture, errMsg, source.Name));
                }
                
            });
            return source;
        }

        internal static IAssertion<T> IsFalse<T>(this Assertion<T> source, Func<T, bool> assertFunc, string errMsg)
        {
            if (assertFunc == null)
            {
                throw new ArgumentNullException("assertFunc is null");
            }

            

            source.Extend(x =>
            {
                if (errMsg == null)
                {
                    return AssertResult.New(!assertFunc(x), errMsg);
                }
                else {
                    errMsg = errMsg.Replace("<name>", "{0}");
                    return AssertResult.New(!assertFunc(x), String.Format(CultureInfo.InvariantCulture, errMsg, source.Name));
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


