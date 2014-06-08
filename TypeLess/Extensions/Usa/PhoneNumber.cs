using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeLess.Algorithms;
using TypeLess.DataTypes;
using TypeLess.Helpers;
using TypeLess.Properties;

namespace TypeLess.Extensions.Usa
{
    public static class PhoneNumber
    {
        private static string _regex = "^[01]?[- .]?(\\([2-9]\\d{2}\\)|[2-9]\\d{2})[- .]?\\d{3}[- .]?\\d{4}$";

        public static IStringAssertion IsNotValidPhoneNumber(this IStringAssertionU source) {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);

                return AssertResult.New(!isValid, Resources.IsNotValidPhoneNumber);
            });
            return (IStringAssertion)source;
        }

        public static IStringAssertion IsValidPhoneNumber(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);
                return AssertResult.New(isValid, Resources.IsValidPhoneNumber);
            });
            return (IStringAssertion)source;
        }

    }
}