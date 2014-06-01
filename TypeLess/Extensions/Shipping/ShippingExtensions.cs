using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Extensions.Shipping
{
    public static class ShippingExtensions
    {

        public static IStringAssertion IsNotValidImoNr(this IStringAssertionU source)
        {
            source.Extend(x =>
            {
                if (!ImoValidator.IsValid(x))
                {
                    return String.Format(CultureInfo.InvariantCulture, "must be a valid IMO number");
                }
                
                return null;
            });

            return (IStringAssertion)source;
        }

    }
}
