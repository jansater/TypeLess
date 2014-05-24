using System;
using System.Text;

namespace RS.Assert
{
    public class Assertion<T> : IAssertion
    {

        private StringBuilder _sb { get; set; }
        public T Item { get; set; }

        public string Name { get; set; }

        private string _file;
        private int? _lineNr;
        private string _caller;

        public Assertion(string s, T source, string file, int? lineNumber, string caller)
        {
            Name = s;
            _sb = new StringBuilder();
            _sb.Append(Name);
            Item = source;

            _file = file;
            _lineNr = lineNumber;
            _caller = caller;
        }

        public Assertion<T> Combine(IAssertion otherAssertion) {

            _isValid |= otherAssertion.IsValid;
            _sb.Append(". ").AppendLine(otherAssertion.ToString());
            return this;
        }

        public void ThenThrow<E>() where E : Exception
        {
            if (IsValid)
            {
                return;
            }
            
#if DEBUG
            throw (Exception)Activator.CreateInstance(typeof(E), new object[] { AppendTrace() });
            
#else
            throw (Exception)Activator.CreateInstance(typeof(E), new object[] { _sb.ToString() });
#endif
        }

        /// <summary>
        /// Throws arg null instead of arg exception just because of the message created
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void ThenThrow()
        {
            if (IsValid)
            {
                return;
            }

#if DEBUG
            throw new ArgumentNullException("", AppendTrace());
#else
            throw new ArgumentNullException("", _sb.ToString());
#endif
        }

        public override string ToString()
        {
            if (IsValid)
            {
                return _sb.ToString() + " is valid";
            }
#if DEBUG
            return AppendTrace();
#else
            return _sb.ToString();
#endif
        }

        private bool _isValid = true;
        public bool IsValid { get { return _isValid; } }

        private string AppendTrace()
        {
            return String.Format("{0} at {1}, line number {2} in file {3} ", _sb.ToString(), _caller, _lineNr, _file);
        }

        private bool _ignoreFurtherChecks = false;

        public bool IgnoreFurtherChecks
        {
            get
            {
                return _ignoreFurtherChecks;
            }
        }

        public Assertion<T> StopIfNotValid()
        {
            _ignoreFurtherChecks = true;
            return this;
        }

        private int _errorCount;
        /// <summary>
        /// The number if validation errors.
        /// </summary>
        /// <returns></returns>
        public int ErrorCount {
            get {
                return _errorCount;
            }
        }

        public void Append(string s)
        {
            _errorCount++;
            if (_isValid)
            {
                _sb.Append(" ").Append(s);
            }
            else
            {
                _sb.Append(" and ").Append(s);
            }
            _isValid = false;
        }

    }
}
