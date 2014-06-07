using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.DataTypes;

namespace TypeLess.Extensions.Shipping
{
    public static class ShippingExtensions
    {

        public static IStringAssertion IsNotValidImoNr(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                return AssertResult.New(!ImoValidator.IsValid(x), "<name> must be a valid IMO number");
            });

            return (IStringAssertion)source;
        }

    }
}
