using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Helpers
{
    public static class IntExtensions
    {
        /// <summary>
        /// 1 based index
        /// </summary>
        /// <param name="n"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static int GetNthDigit(this int n, int digit)
        {
            return (n / (int)Math.Pow(10, digit - 1)) % 10;
        }

        public static int[] ToArray(this int n)
        {
            int _n = Math.Abs(n);
            int length = ((int)Math.Log10(_n > 0 ? _n : 1)) + 1;
            int[] digits = new int[length];
            for (int i = 0; i < length; i++)
            {
                digits[(length - i) - 1] = _n % 10;
                _n /= 10;
            }
            if (n < 0)
                digits[0] *= -1;

            return digits;

        }
    }
}
