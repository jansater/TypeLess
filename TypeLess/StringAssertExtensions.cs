
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
            source.Extend(x => AssertResult.New(x.Length <= 0, "<name> must be non empty"));
            return source;
        }

        internal static StringAssertion IsEmptyOrWhitespace(this StringAssertion source)
        {
            source.Extend(x => AssertResult.New(string.IsNullOrWhiteSpace(x), "<name> must not be empty"));
            return source;
        }

        internal static StringAssertion IsNotValidEmail(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isEmail = Regex.IsMatch(x, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

                return AssertResult.New(string.IsNullOrWhiteSpace(x) || !isEmail, "<name> must be a valid email address");
            });

            return source;
        }

        internal static StringAssertion IsShorterThan(this StringAssertion source, int length)
        {
            source.Extend(x => AssertResult.New(x.Length < length, "<name> must be longer than {0} characters", length - 1));
            return source;
        }

        internal static StringAssertion IsLongerThan(this StringAssertion source, int length)
        {
            source.Extend(x => AssertResult.New(x.Length > length, "<name> must be shorter than {0} characters", length + 1));
            return source;
        }

        internal static StringAssertion DoesNotContainAlphaChars(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isValid = Regex.IsMatch(x, @"\W|_");
                return AssertResult.New(!isValid, "<name> must contain alpha numeric characters");
            });
            return source;
        }

        internal static StringAssertion DoesNotContainDigit(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isValid = Regex.IsMatch(x, @"\d");
                return AssertResult.New(!isValid, "<name> must contain at least 1 digit");
            });
            return source;
        }

        internal static IStringAssertion DoesNotContain(this StringAssertion source, string text)
        {
            source.Extend(x => AssertResult.New(!x.Contains(text), "<name> must contain text " + text));

            return source;
        }

        internal static IStringAssertion DoesNotStartWith(this StringAssertion source, string text)
        {
            source.Extend(x => AssertResult.New(!x.StartsWith(text, StringComparison.CurrentCultureIgnoreCase), "<name> must start with text " + text));

            return source;

        }

        internal static IStringAssertion DoesNotEndWith(this StringAssertion source, string text)
        {
            source.Extend(x => AssertResult.New(!x.EndsWith(text, StringComparison.CurrentCultureIgnoreCase),
                "<name> must end with text " + text));

            return source;
        }
    }
}


