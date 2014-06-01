using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess;
using TypeLess.Helpers;

namespace TypeLess.Algorithms
{
    public static class Luhn
    {
        public static bool IsValid(params int[] arr)
        {
            arr.If("arr").IsNull.ThenThrow();

            var vRot = new ValueRotator<int>(1, 2);
            int controlSum = 0;
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                var m = (arr[i] * vRot.Get());
                if (m > 9)
                {
                    controlSum += m - 9;
                }
                else {
                    controlSum += m;
                }
            }

            var isValid = (controlSum % 10) == 0;
            if (!isValid) {
                return false;   
            }

            vRot = new ValueRotator<int>(2, 1);
            var sum = 0;
            for (int i = arr.Length - 2; i >= 0; i--)
            {
                var m = (arr[i] * vRot.Get());
                if (m > 9)
                {
                    sum += m - 9;
                }
                else
                {
                    sum += m;
                }
            }
            var mod10 = (sum % 10);
            if (mod10 == 0) {
                return arr.Last() == 0;
            }
            var checkDigit = 10 - mod10;
            return checkDigit == arr.Last();
        }
    }
}
