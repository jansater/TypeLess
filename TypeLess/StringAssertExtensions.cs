using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
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
    public static class StringAssertExtensions
    {

        internal static StringAssertion IsEmpty(this StringAssertion source)
        {
            source.Extend(x => AssertResult.New(x.Length <= 0, Resources.IsEmpty));
            return source;
        }

        internal static StringAssertion IsEmptyOrWhitespace(this StringAssertion source)
        {
            source.Extend(x => AssertResult.New(string.IsNullOrWhiteSpace(x), Resources.IsEmpty));
            return source;
        }

        internal static StringAssertion IsNotValidEmail(this StringAssertion source)
        {
            source.Extend(x =>
            {
                //bool isEmail = Regex.IsMatch(x, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");

                RegexHelper helper = new RegexHelper();
                var isEmail = helper.IsValidEmail(x);

                return AssertResult.New(string.IsNullOrWhiteSpace(x) || !isEmail, Resources.IsNotValidEmail);
            });

            return source;
        }

        internal static StringAssertion IsShorterThan(this StringAssertion source, int length)
        {
            source.Extend(x => AssertResult.New(x.Length < length, Resources.IsShorterThan, length - 1));
            return source;
        }

        internal static StringAssertion IsLongerThan(this StringAssertion source, int length)
        {
            source.Extend(x => AssertResult.New(x.Length > length, Resources.IsLongerThan, length + 1));
            return source;
        }

        internal static StringAssertion DoesNotContainAlphaChars(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isValid = Regex.IsMatch(x, @"\W|_");
                return AssertResult.New(!isValid, Resources.DoesNotContainAlphaChars);
            });
            return source;
        }

        internal static StringAssertion DoesNotContainDigit(this StringAssertion source)
        {
            source.Extend(x =>
            {
                bool isValid = Regex.IsMatch(x, @"\d");
                return AssertResult.New(!isValid, Resources.DoesNotContainDigit);
            });
            return source;
        }

        internal static IStringAssertion DoesNotContain(this StringAssertion source, string text)
        {
            source.Extend(x => AssertResult.New(!x.Contains(text), Resources.DoesNotContain, text));

            return source;
        }

        internal static IStringAssertion DoesNotStartWith(this StringAssertion source, string text)
        {
            source.Extend(x => AssertResult.New(!x.StartsWith(text, StringComparison.CurrentCultureIgnoreCase), Resources.DoesNotStartWith, text));

            return source;

        }

        internal static IStringAssertion DoesNotEndWith(this StringAssertion source, string text)
        {
            source.Extend(x => AssertResult.New(!x.EndsWith(text, StringComparison.CurrentCultureIgnoreCase),
                Resources.DoesNotEndWith, text));

            return source;
        }
    }
}


