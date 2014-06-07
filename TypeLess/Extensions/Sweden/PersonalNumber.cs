using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.Algorithms;
using TypeLess.DataTypes;
using TypeLess.Helpers;

namespace TypeLess.Extensions.Sweden
{
    public static class PersonalNumber
    {

        public static IStringAssertion IsNotValidPersonalNumber(this IStringAssertionU source) {
            source.Extend(x =>
            {
                return AssertResult.New(!Luhn.IsValid(x.ToIntArray()), "<name> must be a valid personal number");
            });
            return (IStringAssertion)source;
        }

        public static IStringAssertion IsValidPersonalNumber(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                return AssertResult.New(Luhn.IsValid(x.ToIntArray()), "<name> must not be a valid personal number");
            });
            return (IStringAssertion)source;
        }

    }
}
