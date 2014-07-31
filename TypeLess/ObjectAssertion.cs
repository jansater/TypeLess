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
            if (assertions == null)
            {
                throw new ArgumentNullException("assertions");
            }

            this.Assertions = assertions;
        }

        public static ObjectAssertion New(params IAssertion[] assertions)
        {
            if (assertions == null || !assertions.Any())
            {
                throw new ArgumentNullException("You must at least define 1 assertion");
            }

            return new ObjectAssertion(assertions);
        }

        public string ToString(out int errCount)
        {
            var sb = new StringBuilder();
            errCount = 0;

            foreach (var item in Assertions)
            {
                var s = item.ToString(skipTrace: true);
                if (errCount > 0 && !String.IsNullOrWhiteSpace(s))
                {
                    sb.Append(" and ").Append(s);
                    errCount += item.ErrorCount;
                }
                else
                {
                    errCount += item.ErrorCount;
                    sb.Append(s);
                }
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            int errors = 0;
            return ToString(out errors);
        }

        /// <summary>
        /// Returns true if all assertions are valid
        /// </summary>
        public bool True
        {
            get
            {
                foreach (var item in Assertions)
                {
                    if (item.False)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// returns true if any assertion is invalid
        /// </summary>
        public bool False
        {
            get
            {
                foreach (var item in Assertions)
                {
                    if (item.False)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void ThrowIfFalse<TException>()
        {
            if (False)
            {
                throw (Exception)Activator.CreateInstance(typeof(TException), new object[] { ToString() });
            }
        }

        public void ThrowIfTrue<TException>()
        {
            if (True)
            {
                throw (Exception)Activator.CreateInstance(typeof(TException), new object[] { ToString() });
            }
        }
    }
}
