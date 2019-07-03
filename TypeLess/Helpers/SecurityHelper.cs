using System.Security.Cryptography;
using System.Text;

namespace TypeLess.Net
{
    public class SecurityHelper
    {
        
        public static string HashString(string input) {
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
            return ByteArrayToString(hash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
