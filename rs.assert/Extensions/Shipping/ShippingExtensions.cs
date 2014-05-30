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

        public static IStringAssertion IsNotValidImoNr<T>(this T source) where T : IStringAssertionU
        {
            source.Extend((s) =>
            {
                if (!ImoValidator.IsValid(s))
                {
                    return String.Format(CultureInfo.InvariantCulture, "must be a valid IMO number");
                }
                
                return null;
            }, (s) => IsNotValidImoNr((IStringAssertion)s));

            return (IStringAssertion)source;
        }

    }
}
