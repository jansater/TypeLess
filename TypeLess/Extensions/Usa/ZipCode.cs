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
    public static class ZipCode
    {
        private static string _regex = "^(\\d{5}-\\d{4}|\\d{5}|\\d{9})$|^([a-zA-Z]\\d[a-zA-Z] \\d[a-zA-Z]\\d)$";

        public static IStringAssertion IsNotValidZipCode(this IStringAssertionU source) {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);

                return AssertResult.New(!isValid, Resources.IsNotValidZipCode);
            });
            return (IStringAssertion)source;
        }

        public static IStringAssertion IsValidZipCode(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);
                return AssertResult.New(isValid, Resources.IsValidZipCode);
            });
            return (IStringAssertion)source;
        }

    }
}