using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess
{

    public class AssignmentException : ArgumentNullException
    {
        public AssignmentException() { }
        public AssignmentException(string message) : base(message) { }
        public AssignmentException(string message, Exception inner) : base(message, inner) { }

        public override string ToString()
        {
            return "Assignment exception: " + Message;
        }
    }
}
