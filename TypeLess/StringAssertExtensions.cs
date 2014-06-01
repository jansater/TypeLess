
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
            source.Extend(x => x.Length <= 0 ? "must be non empty" : null);
            return source;
        }

        internal static StringAssertion IsEmptyOrWhitespace(this StringAssertion source)
        {
            source.Extend(x => string.IsNullOrWhiteSpace(x) ? "must not be empty" : null);
            return source;
        }

        internal static StringAssertion IsNotValidEmail(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isEmail = Regex.IsMatch(x, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                if (string.IsNullOrWhiteSpace(x) || !isEmail)
                {
                    return "must be a valid email address";
                }
                return null;
            });

            return source;
        }

        internal static StringAssertion IsShorterThan(this StringAssertion source, int length)
        {
            source.Extend(x => x.Length < length ? String.Format(CultureInfo.InvariantCulture, "must be longer than {0} characters", length - 1) : null);
            return source;
        }

        internal static StringAssertion IsLongerThan(this StringAssertion source, int length)
        {
            source.Extend(x => x.Length > length ? "must be shorter than {0} characters" : null);

            return source;
        }

        internal static StringAssertion DoesNotContainAlphaChars(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isValid = Regex.IsMatch(x, @"\W|_");
                if (!isValid)
                {
                    return "must contain alpha numeric characters";
                }
                return null;
            });
            return source;
        }

        internal static StringAssertion DoesNotContainDigit(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isValid = Regex.IsMatch(x, @"\d");
                if (!isValid)
                {
                    return "must contain at least 1 digit";
                }
                return null;
            });
            return source;
        }

        internal static IStringAssertion DoesNotContain(this StringAssertion source, string text)
        {
            source.Extend(x => !x.Contains(text) ? "must contain text " + text : null);

            return source;
        }

        internal static IStringAssertion DoesNotStartWith(this StringAssertion source, string text)
        {
            source.Extend(x => !x.StartsWith(text, StringComparison.CurrentCultureIgnoreCase) ? "must start with text " + text : null);

            return source;

        }

        internal static IStringAssertion DoesNotEndWith(this StringAssertion source, string text)
        {
            source.Extend(x => !x.EndsWith(text, StringComparison.CurrentCultureIgnoreCase)
                ? "must start with text " + text : null);

            return source;
        }
    }
}


