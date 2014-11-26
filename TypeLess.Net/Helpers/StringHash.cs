using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Net.Helpers
{
    public static class StringHash
    {
        public static string HashString(string input)
        {
            return ByteArrayToString(new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            StringBuilder builder = new StringBuilder(arrInput.Length);
            for (int i = 0; i < arrInput.Length; i++)
            {
                builder.Append(arrInput[i].ToString("X2"));
            }
            return builder.ToString();
        }


    }
}
