using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess
{
#if !DEBUG
    [DebuggerStepThrough]
#endif
    public class ObjectAssertion
    {
        private List<IAssertion> _assertions = new List<IAssertion>();

        public IEnumerable<IAssertion> Assertions
        {
            get
            {
                return _assertions;
            }
            set
            {
                if (value == null)
                {
                    _assertions = null;
                }
                else
                {
                    _assertions = value.ToList();
                }
            }
        }

        public ObjectAssertion(params IAssertion[] assertions)
        {
            if (assertions == null)
            {
                throw new ArgumentNullException("assertions");
            }


            foreach (var assertion in assertions)
            {
                if (assertion != null)
                {
                    _assertions.Add(assertion);
                }
            }

        }

        public void AddAssertions(params IAssertion[] assertions)
        {
            if (assertions == null)
            {
                throw new ArgumentNullException("assertions");
            }

            foreach (var assertion in assertions)
            {
                if (assertion != null)
                {
                    _assertions.Add(assertion);
                }
            }

        }

        public void RemoveAssertion(IAssertion assertion)
        {
            if (assertion == null)
            {
                throw new ArgumentNullException("assertions");
            }
            _assertions.Remove(assertion);
        }

        public static ObjectAssertion New(params IAssertion[] assertions)
        {
            if (assertions == null)
            {
                throw new ArgumentNullException("assertions");
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

        public IDictionary<string, string> ToDictionary(string separator = null) {
            var dict = new Dictionary<string, string>();

            foreach (var item in Assertions)
            {
                var s = item.ToString(skipTrace: true);
                if (item.ErrorCount > 0 && !String.IsNullOrWhiteSpace(s) && !String.IsNullOrEmpty(item.Name))
                {
                    if (dict.ContainsKey(item.Name))
                    {
                        var current = dict[item.Name];
                        if (separator != null)
                        {
                            current = current + separator + s;
                        }
                        else
                        {
                            current = current + ". " + s;
                        }
                        dict[item.Name] = current;
                    }
                    else {
                        dict[item.Name] = s;
                    }
                }
            }

            return dict;
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
                //this means that at least 1 of the business rules are invalid = false ... and we only want to get that message in our exeption
                //but as defined in the rest of the lib ToString only contains messages when the condition is invalid ... so tostring in this case returns 
                //the wrong information
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
