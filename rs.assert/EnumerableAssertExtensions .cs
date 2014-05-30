
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
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            var c = GetCount(source.Item);

            if (c == 0)
            {
                source.Append("must be non empty");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.IsEmpty());
            }

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
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            var count = GetCount(source.Item);

            if (count < nElements)
            {
                source.Append("must contain more than " + nElements + " items");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.ContainsLessThan(nElements));
            }

            return source;
        }

        internal static EnumerableAssertion ContainsMoreThan(this EnumerableAssertion source, int nElements)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            var count = GetCount(source.Item);

            if (count > nElements)
            {
                source.Append("must contain less than " + nElements + " items");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.ContainsMoreThan(nElements));
            }

            return source;
        }

    }
}


