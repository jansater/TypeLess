using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace TypeLess
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFluentInterface
    {
        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetType()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type GetType();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.GetHashCode()"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();

        /// <summary>
        /// Redeclaration that hides the <see cref="object.Equals(object)"/> method from IntelliSense.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }


    public interface IAssertion : IFluentInterface
    {
        bool IsValid { get; }
    }

    public interface ICompleteAssertion
    {
        int ErrorCount { get; }
        bool IgnoreFurtherChecks { get; }
        void ThenThrow();
        void ThenThrow<E>() where E : Exception;
        string ToString();
    }

    public interface IAssertion<T> : IAssertion
    {
        IAssertion Combine(IAssertion otherAssertion);
        //IAssertion<T> Or<S>(S obj, string withName = null);
        IAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse);
        IAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue);
        IAssertion<T> IsNotEqualTo(T comparedTo);
        IAssertion<T> IsEqualTo(T comparedTo);
    }


#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class Assertion<T> : TypeLess.IAssertion<T>
    {
        private StringBuilder _sb { get; set; }
        internal T Item { get; set; }

        internal string Name { get; set; }

        private string _file;
        private int? _lineNr;
        private string _caller;
        private List<Assertion<object>> _childAssertions = new List<Assertion<object>>();

        public Assertion(string s, T source, string file, int? lineNumber, string caller)
        {
            Name = s;
            _sb = new StringBuilder();
            Item = source;

            _file = file;
            _lineNr = lineNumber;
            _caller = caller;
        }

        internal Assertion<object> Add(object obj, string name)
        {
            //todo re add this

            var assert = AssertExtensions.CreateAssert(obj, name);
            var tObj = this.Cast<object>();
            assert._childAssertions.Add(tObj);
            return assert;
        }

        public Assertion<T> StopIfNotValid
        {
            get
            {
                IgnoreFurtherChecks = true;
                return this;
            }

        }

        internal Assertion<S> Cast<S>()
        {
            if (this.Item == null)
            {
                return new Assertion<S>(Name, default(S), null, null, null)
                {
                    _sb = this._sb,
                    _caller = this._caller,
                    _file = this._file,
                    _childAssertions = this._childAssertions,
                    _errorCount = this._errorCount,
                    _ignoreFurtherChecks = this._ignoreFurtherChecks,
                    _isValid = this._isValid,
                    _lineNr = this._lineNr,
                };
            }

            var item = this.Item as object;

            if (!(item is S))
            {
                throw new InvalidCastException("You can't mix types " + typeof(S).Name + " and " + typeof(T));
            }
            var s = (S)item;

            return new Assertion<S>(Name, s, null, null, null)
            {
                _sb = this._sb,
                _caller = this._caller,
                _file = this._file,
                _childAssertions = this._childAssertions,
                _errorCount = this._errorCount,
                _ignoreFurtherChecks = this._ignoreFurtherChecks,
                _isValid = this._isValid,
                _lineNr = this._lineNr,
            };
        }

        internal void ClearErrorMsg()
        {
            _sb.Clear();
        }

        internal List<Assertion<object>> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public IAssertion Combine(IAssertion otherAssertion)
        {

            if (otherAssertion == null || otherAssertion.IsValid)
            {
                return this;
            }

            if (_isValid)
            {
                _isValid &= otherAssertion.IsValid;
                _sb.Append(otherAssertion.ToString());
            }
            else
            {
                _isValid &= otherAssertion.IsValid;
                _sb.Append(". ").AppendLine(otherAssertion.ToString());
            }

            return this;
        }

        public void ThenThrow<E>() where E : Exception
        {
            if (IsValid)
            {
                return;
            }

            if (Debugger.IsAttached)
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { AppendTrace() });
            }
            else
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { _sb.ToString() });
            }

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

            if (Debugger.IsAttached && _errorCount != 0)
            {
                throw new ArgumentNullException("", AppendTrace());
            }
            else
            {
                throw new ArgumentNullException("", _sb.ToString());
            }
        }

        public override string ToString()
        {
            if (IsValid)
            {
                return Name + " is valid";
            }

            if (Debugger.IsAttached && _errorCount != 0)
            {
                return AppendTrace();
            }
            else
            {
                return _sb.ToString();
            }
        }

        private bool _isValid = true;
        public bool IsValid { get { return _isValid; } }

        private string AppendTrace()
        {
            if (_caller == null || _file == null || !_lineNr.HasValue)
            {
                return _sb.ToString();
            }
            else
            {
                return String.Format("{0} at {1}, line number {2} in file {3} ", _sb.ToString(), _caller, _lineNr, _file);
            }
        }

        private bool _ignoreFurtherChecks = false;

        public bool IgnoreFurtherChecks
        {
            get
            {
                return _ignoreFurtherChecks;
            }
            internal set
            {
                _ignoreFurtherChecks = value;
            }
        }

        private int _errorCount;

        /// <summary>
        /// The number if validation errors.
        /// </summary>
        /// <returns></returns>
        public int ErrorCount
        {
            get
            {
                return _errorCount;
            }
        }

        internal void Append(string s)
        {
            if (_errorCount == 0)
            {
                _sb.Append(Name);
            }

            _errorCount++;

            if (_isValid)
            {
                _sb.Append(" ").Append(s);
            }
            else if (ChildAssertions.Count > 0)
            {
                _sb.AppendFormat(" and {0} ", Name).Append(s);
            }
            else
            {
                _sb.Append(" and ").Append(s);
            }
            _isValid = false;
        }



        public IAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse)
        {
            return AssertExtensions.IsTrue(this, assertFunc, msgIfFalse);
        }

        public IAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue)
        {
            return AssertExtensions.IsFalse(this, assertFunc, msgIfTrue);
        }

        public IAssertion<T> IsNotEqualTo(T comparedTo)
        {
            return AssertExtensions.IsNotEqualTo(this, comparedTo);
        }

        public IAssertion<T> IsEqualTo(T comparedTo)
        {
            return AssertExtensions.IsEqualTo(this, comparedTo);
        }
    }
}
