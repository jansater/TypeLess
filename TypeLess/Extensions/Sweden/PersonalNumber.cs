using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.Algorithms;
using TypeLess.DataTypes;
using TypeLess.Helpers;
using TypeLess.Properties;

namespace TypeLess.Extensions.Sweden
{
    public static class PersonalNumber
    {

        public static IStringAssertion IsNotValidPersonalNumber(this IStringAssertionU source) {
            source.Extend(x =>
            {
                return AssertResult.New(!Luhn.IsValid(x.ToIntArray()), Resources.IsNotValidPersonalNumber);
            });
            return (IStringAssertion)source;
        }

        public static IStringAssertion IsValidPersonalNumber(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                return AssertResult.New(Luhn.IsValid(x.ToIntArray()), Resources.IsValidPersonalNumber);
            });
            return (IStringAssertion)source;
        }

    }
}
