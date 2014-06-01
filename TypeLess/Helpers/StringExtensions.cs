using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess;

namespace TypeLess.Helpers
{
    public static class StringExtensions
    {
        public static int[] ToIntArray(this string s) {
            s.If("s").IsEmptyOrWhitespace.ThenThrow();

            var cArr = s.ToCharArray();
            int[] arr = new int[cArr.Length];
            for (int i = 0; i < cArr.Length; i++)
            {
                if (!char.IsNumber(cArr[i]))
                {
                    throw new ArgumentException("Index " + i + " is not a number");
                }

                arr[i] = (int)char.GetNumericValue(cArr[i]);
            }

            return arr;
        }
    }
}
