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
    public static class SocialSecurityNumber
    {
        private static string _regex = "^\\d{3}-\\d{2}-\\d{4}$";

        public static IStringAssertion IsNotValidSocialSecurityNumber(this IStringAssertionU source) {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);

                return AssertResult.New(!isValid, Resources.IsNotValidSocialSecurityNumber);
            });
            return (IStringAssertion)source;
        }

        public static IStringAssertion IsValidSocialSecurityNumber(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);
                return AssertResult.New(isValid, Resources.IsValidSocialSecurityNumber);
            });
            return (IStringAssertion)source;
        }

    }
}