using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Extensions.Shipping
{
  
    internal static class ImoValidator
    {

        private static int ToInt(this char c)
        {
            return (int)(c - '0');
        }

         /// <summary>
        /// The IMO ship identification number is made of the three letters "IMO" followed by the seven-digit number. This consists of a six-digit sequential unique number followed by a check digit. The integrity of an IMO number can be verified using its check digit. This is done by multiplying each of the first six digits by a factor of 2 to 7 corresponding to their position from right to left. The rightmost digit of this sum is the check digit. For example, for IMO 9074729: (9×7) + (0×6) + (7×5) + (4×4) + (7×3) + (2×2) = 139
        /// </summary>
        /// <param name="imo"></param>
        /// <returns></returns>
        public static bool IsValid(string imo) {
            if (String.IsNullOrWhiteSpace(imo)) {
                return false;
            }

            imo = imo.ToLower().Replace("imo", String.Empty).Replace(" ", String.Empty);

            var chars = imo.ToCharArray();
            if (chars.Length != 7) {
                return false;
            }

            int sum = 0;
            for (int i = 0; i < chars.Length - 1; i++)
            {
                sum += chars[i].ToInt() * (chars.Length - i);
            }

            var s = sum.ToString();
            var checkDigit = s[s.Length - 1];
            return imo[imo.Length - 1] == checkDigit;
        }

    }
}
