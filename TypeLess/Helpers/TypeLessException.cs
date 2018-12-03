using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Helpers
{

    public class TypeLessException : Exception
    {
        public TypeLessException() { }
        public TypeLessException(string message) : base(message) { }
        public TypeLessException(string message, Exception inner) : base(message, inner) { }
    }
}
