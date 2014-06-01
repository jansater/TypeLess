using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess
{
    public class ObjectAssertion 
    {
        public IEnumerable<IAssertion> Assertions { get; set; }

        public ObjectAssertion(params IAssertion[] assertions)
        {
            assertions.If("assertions").IsNull.ThenThrow();
            
            this.Assertions = assertions;
        }

        public static ObjectAssertion New(params IAssertion[] assertions) {
            return new ObjectAssertion(assertions);
        } 
    }
}
