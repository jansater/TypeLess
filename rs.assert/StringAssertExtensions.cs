
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

namespace TypeLess
{

    /// <summary>
    /// Throws arg null exception instead of arg exception just to avoid parameter name messages ... could use a custom exception though
    /// </summary>
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public static class StringAssertExtensions 
    {

        internal static StringAssertion IsEmpty(this StringAssertion source)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (source.Item.Length <= 0)
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

        internal static StringAssertion IsEmptyOrWhitespace(this StringAssertion source)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (string.IsNullOrWhiteSpace(source.Item))
            {
                source.Append("must not be empty");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.IsEmptyOrWhitespace);
            }

            return source;
        }

        internal static StringAssertion IsNotValidEmail(this StringAssertion source)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            bool isEmail = Regex.IsMatch(source.Item, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
            if (string.IsNullOrWhiteSpace(source.Item) || !isEmail)
            {
                source.Append("must be a valid email address");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.IsNotValidEmail);
            }

            return source;
        }

        internal static StringAssertion IsShorterThan(this StringAssertion source, int length)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (source.Item.Length < length)
            {
                source.Append(String.Format(CultureInfo.InvariantCulture, "must be longer than {0} characters", length - 1));
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.IsShorterThan(length));
            }
            return source;
        }

        internal static StringAssertion IsLongerThan(this StringAssertion source, int length)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (source.Item.Length > length)
            {
                source.Append(String.Format(CultureInfo.InvariantCulture, "must be shorter than {0} characters", length + 1));
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.IsLongerThan(length));
            }

            return source;
        }

        internal static StringAssertion DoesNotContainAlphaChars(this StringAssertion source)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            bool isValid = Regex.IsMatch(source.Item, @"\W|_");
            if (!isValid)
            {
                source.Append("must contain alpha numeric characters");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.DoesNotContainAlphaChars);
            }

            return source;
        }

        internal static StringAssertion DoesNotContainDigit(this StringAssertion source)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            bool isValid = Regex.IsMatch(source.Item, @"\d");
            if (!isValid)
            {
                source.Append("must contain at least 1 digit");
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.DoesNotContainDigit());
            }

            return source;
        }

        internal static IStringAssertion DoesNotContain(this StringAssertion source, string text)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (!source.Item.Contains(text))
            {
                source.Append("must contain text " + text);
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.DoesNotContain(text));
            }

            return source;
        }

        internal static IStringAssertion DoesNotStartWith(this StringAssertion source, string text)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (!source.Item.StartsWith(text, StringComparison.CurrentCultureIgnoreCase))
            {
                source.Append("must start with text " + text);
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.DoesNotStartWith(text));
            }

            return source;
        }

        internal static IStringAssertion DoesNotEndWith(this StringAssertion source, string text)
        {
            if (source.IgnoreFurtherChecks)
            {
                return source;
            }

            if (!source.Item.EndsWith(text, StringComparison.CurrentCultureIgnoreCase))
            {
                source.Append("must start with text " + text);
            }

            foreach (var child in source.ChildAssertions)
            {
                child.ClearErrorMsg();
                source.Combine(child.DoesNotEndWith(text));
            }

            return source;
        }
    }
}


