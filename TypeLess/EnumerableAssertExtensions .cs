
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
using TypeLess.DataTypes;

namespace TypeLess
{

    /// <summary>
    /// Throws arg null exception instead of arg exception just to avoid parameter name messages ... could use a custom exception though
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public static class EnumerableAssertExtensions 
    {

        internal static EnumerableAssertion IsEmpty(this EnumerableAssertion source)
        {
            source.Extend(x =>
            {
                var c = GetCount(x);
                return AssertResult.New(c == 0, "<name> must be non empty");
            });
            return source;
        }

        internal static int GetCount(IEnumerable e)
        {
            var asList = e as IList;
            if (asList != null)
            {
                return asList.Count;
            }

            var enu = e.GetEnumerator();

            if (!enu.MoveNext())
            {
                return 0;
            }
            int i = 1;
            for (; enu.MoveNext(); i++) ;
            return i;
        }

        internal static EnumerableAssertion ContainsLessThan(this EnumerableAssertion source, int nElements)
        {
            source.Extend(x =>
            {
                var count = GetCount(x);
                return AssertResult.New(count < nElements, "<name> must contain more than " + nElements + " items");
            });
            return source;
        }

        internal static EnumerableAssertion ContainsMoreThan(this EnumerableAssertion source, int nElements)
        {
            source.Extend(x =>
            {
                var count = GetCount(x);
                return AssertResult.New(count > nElements, "<name> must contain less than " + nElements + " items");
            });
            return source;
        }

    }
}


