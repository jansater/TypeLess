using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TypeLess.DataTypes
{
    public class AssertResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        
        private AssertResult(bool isValid, string msg, params object[] parameters) {
            this.IsValid = isValid;

            if (msg != null && parameters != null && parameters.Any())
            {
                this.Message = String.Format(CultureInfo.InvariantCulture, msg, parameters);
            }
            else {
                this.Message = msg;
            }         
        }

        public static AssertResult New(bool isValid, string ifInvalidMessage = null, params object[] parameters) {
            return new AssertResult(isValid, ifInvalidMessage, parameters);
        }

        public static AssertResult False {
            get {
                return new AssertResult(false, null);
            }
        }

    }
}
