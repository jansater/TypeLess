using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess.Extensions.Shipping
{
    public static class ShippingExtensions
    {

        public static IStringAssertion IsNotValidImoNr(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                return AssertResult.New(!ImoValidator.IsValid(x), Resources.IsNotValidImoNr);
            });

            return (IStringAssertion)source;
        }

    }
}
