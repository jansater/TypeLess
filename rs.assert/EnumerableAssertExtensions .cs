
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

                if (c == 0)
                {
                    return "must be non empty";
                }
                return null;
            }, x => source);
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

                if (count < nElements)
                {
                    return "must contain more than " + nElements + " items";
                }
                return null;
            }, x => source);
            return source;
        }

        internal static EnumerableAssertion ContainsMoreThan(this EnumerableAssertion source, int nElements)
        {
            source.Extend(x =>
            {
                var count = GetCount(x);

                if (count > nElements)
                {
                    return "must contain less than " + nElements + " items";
                }
                return null;
            }, x => source);
            return source;
        }

    }
}


