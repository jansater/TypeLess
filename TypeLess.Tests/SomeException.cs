using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Tests
{
    [Serializable]
    public class SomeException : Exception
    {
        public SomeException() { }
        public SomeException(string message) : base(message) { }
        public SomeException(string message, Exception inner) : base(message, inner) { }
        protected SomeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
