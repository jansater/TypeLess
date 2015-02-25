using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess.Extensions.Banks
{
    /*
     AAAA BB CC DDD

    First 4 characters - bank code (only letters)
    Next 2 characters - ISO 3166-1 alpha-2 country code (only letters)
    Next 2 characters - location code (letters and digits) (passive participant will have "1" in the second character)
    Last 3 characters - branch code, optional ('XXX' for primary office) (letters and digits)
     
      
       ^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$
       ^          ^           ^  ^
       |          |           |  |
       6 letters  2 letters   3 letters or digits
                  or digits      |
                                 last three are optional
     */
    public static class Swift
    {
        private static string _regex = "^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$";

        public static IStringAssertion IsNotValidSwiftCode(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                var isValid = Regex.IsMatch(x, _regex);

                return AssertResult.New(!isValid, Resources.IsNotValidSwiftCode);
            });
            return (IStringAssertion)source;
        }

    }
}
