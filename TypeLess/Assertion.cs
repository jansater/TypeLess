using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using TypeLess.DataTypes;

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


    public interface IAssertionU : IFluentInterface
    {
        /// <summary>
        /// Is the current condition true
        /// </summary>
        bool True { get; }
        /// <summary>
        /// Is the current condition false
        /// </summary>
        bool False { get; }

        /// <summary>
        /// Remove any error messages in this assertion
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void ClearErrorMsg();
    }

    public interface IAssertionU<T> : IAssertionU
    {
        IMixedTypeAssertionU<T, S> Or<S>(S obj, string withName = null) where S : class;

        /// <summary>
        /// Checks wether the given statement is true
        /// </summary>
        /// <param name="assertFunc"></param>
        /// <param name="msgIfFalse">Message to return. Use <name> to include the parameter name in the string.</param>
        /// <returns></returns>
        IAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse);
        /// <summary>
        /// Checks wether the given statement is false
        /// </summary>
        /// <param name="assertFunc"></param>
        /// <param name="msgIfFalse">Message to return. Use <name> to include the parameter name in the string.</param>
        /// <returns></returns>
        IAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue);
        IAssertion<T> IsNotEqualTo(T comparedTo);
        IAssertion<T> IsEqualTo(T comparedTo);

        [EditorBrowsable(EditorBrowsableState.Never)]
        void Extend(Func<T, AssertResult> assertFunc);
    }

    public interface IAssertion : IAssertionU
    {
        int ErrorCount { get; }
        bool IgnoreFurtherChecks { get; }
        string ToString(bool skipTrace);
        string ToString();
        IAssertion Or(IAssertion otherAssertion, string separator = ". ");
        
    }

    public interface IAssertionOW<T>
    {
        void Otherwise(Action<T> action);
    }


    public interface IAssertion<T> : IAssertion, IAssertionU<T>
    {
        IAssertion<T> Or<S>(IAssertion<S> otherAssertion, string separator = ". ");

        void Then(Action<T> action);
        S ThenReturn<S>(Func<T, S> func);
        /// <summary>
        /// Throws an argument null exception.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow(string errorMsg = null);
        /// <summary>
        /// Throws an exception of type T.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow<E>(string errorMsg = null) where E : Exception;

    }


#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class Assertion<T> : TypeLess.IAssertion<T>, IAssertion, IAssertionOW<T>
    {
        private StringBuilder _sb { get; set; }
        internal T Item { get; set; }

        internal string Name { get; set; }

        private string _file;
        private int? _lineNr;
        private string _caller;
        private List<Assertion<T>> _children = new List<Assertion<T>>();

        public Assertion(string s, T source, string file, int? lineNumber, string caller)
        {
            Name = s;
            _sb = new StringBuilder();
            Item = source;

            _file = file;
            _lineNr = lineNumber;
            _caller = caller;
        }

        public IMixedTypeAssertionU<T, S> Or<S>(S obj, string withName = null) where S : class
        {
            var mixed = new MixedTypeAssertion<T, S>(withName, Item, obj)
            {
                _sb = this._sb,
                _caller = this._caller,
                _file = this._file,
                _children = this._children,
                _errorCount = this._errorCount,
                _ignoreFurtherChecks = this._ignoreFurtherChecks,
                _isValid = this._isValid,
                _lineNr = this._lineNr,
            };
            mixed._children.Add(this);
            return mixed;
        }

        /// <summary>
        /// Adds a child assertion as or
        /// </summary>
        /// <param name="assertion"></param>
        internal void AddWithOr(Assertion<T> assertion)
        {
            if (assertion == null) {
                throw new ArgumentNullException("assertion is required");
            }
            
            _children.Add(assertion);
        }

        public Assertion<T> StopIfNotValid
        {
            get
            {
                IgnoreFurtherChecks = true;
                return this;
            }

        }

        public void ClearErrorMsg()
        {
            _sb.Clear();
        }

        public IAssertion Or(IAssertion otherAssertion, string separator = ". ")
        {

            if (otherAssertion == null || otherAssertion.ErrorCount == 0)
            {
                return this;
            }

            if (!_isValid)
            {
                _isValid |= otherAssertion.True;
                _sb.Append(otherAssertion.ToString(skipTrace: true));
            }
            else
            {
                _isValid |= otherAssertion.True;
                _sb.Append(separator ?? ". ").AppendLine(otherAssertion.ToString(skipTrace: true));
            }

            return this;
        }

        public IAssertion<T> Or<S>(IAssertion<S> otherAssertion, string separator = ". ")
        {

            if (otherAssertion == null || otherAssertion.ErrorCount == 0)
            {
                return this;
            }

            if (!_isValid)
            {
                _isValid |= otherAssertion.True;
                _sb.Append(otherAssertion.ToString(skipTrace: true));
            }
            else
            {
                _isValid |= otherAssertion.True;
                _sb.Append(separator ?? ". ").AppendLine(otherAssertion.ToString(skipTrace: true));
            }

            return this;
        }

        public IAssertionOW<T> ThenThrow<E>(string errorMsg = null) where E : Exception
        {
            if (!True)
            {
                return this;
            }

            if (Debugger.IsAttached)
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { AppendTrace(errorMsg == null ? String.Format(CultureInfo.InvariantCulture, _sb.ToString().Replace("<name>", "{0}"), Name) : String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name)) });

            }
            else
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg == null ? String.Format(CultureInfo.InvariantCulture, _sb.ToString().Replace("<name>", "{0}"), Name) : String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name) });
            }

        }

        /// <summary>
        /// Throws arg null instead of arg exception just because of the message created
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IAssertionOW<T> ThenThrow(string errorMsg = null)
        {
            if (!True)
            {
                return this;
            }

            if (Debugger.IsAttached && _errorCount != 0)
            {
                throw new ArgumentNullException("", AppendTrace(errorMsg == null ?
                    String.Format(CultureInfo.InvariantCulture, _sb.ToString().Replace("<name>", "{0}"), Name) : 
                    String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name)));
            }
            else
            {
                throw new ArgumentNullException("", errorMsg == null ?
                    String.Format(CultureInfo.InvariantCulture, _sb.ToString().Replace("<name>", "{0}"), Name) : 
                    String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name));
            }
        }

        public override string ToString()
        {
            return this.ToString(skipTrace: false);
        }

        public string ToString(bool skipTrace)
        {
            if (_errorCount <= 0)
            {
                return String.Empty;
            }

            if (Debugger.IsAttached && _errorCount != 0 && !skipTrace)
            {
                return AppendTrace(String.Format(CultureInfo.InvariantCulture, _sb.ToString().Replace("<name>", "{0}"), Name));
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, _sb.ToString().Replace("<name>", "{0}"), Name);
            }
        }

        private bool _isValid = false;
        public bool True { get { return _isValid; } }
        public bool False { get { return !_isValid; } }

        private string AppendTrace(string msg)
        {
            if (_caller == null || _file == null || !_lineNr.HasValue)
            {
                return msg;
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, "{0} at {1}, line number {2} in file {3} ", msg, _caller, _lineNr, _file);
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
            if (String.IsNullOrWhiteSpace(s)) {
                return;
            } 
            
            _errorCount++;

            if (_errorCount <= 1)
            {
                _sb.Append(s);
            }
            else
            {
                _sb.Append(" and ").Append(s);
            }
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

        public void Then(Action<T> action)
        {
            action.If().IsNull.ThenThrow();

            if (!True)
            {
                return;
            }
            action(Item);
        }

        public S ThenReturn<S>(Func<T, S> func)
        {
            if (!True)
            {
                return default(S);
            }

            return func(Item);
        }


        public void Extend(Func<T, AssertResult> assertFunc) //Func<IAssertionU, IAssertion> selfFunc
        {
            if (IgnoreFurtherChecks)
            {
                return;
            }

            var s = assertFunc(Item);

            if (s != null && s.Message != null && s.IsValid)
            {
                Append(s.Message);
            }
            
            _isValid |= s.IsValid;
                
            foreach (var child in _children)
            {
                if (child is Assertion<T>)
                {
                    var childAssertion = (Assertion<T>)child;
                    var res = assertFunc(childAssertion.Item);

                    if (res != null && res.Message != null && res.IsValid)
                    {
                        _sb.Append(". ").Append(String.Format(CultureInfo.InvariantCulture, res.Message.Replace("<name>", "{0}"), childAssertion.Name));
                    }
                    else if (res != null && !res.IsValid)
                    {
                        _isValid |= false;
                    }
                }
            }

        }

        public void Otherwise(Action<T> action)
        {
            action.If("action").IsNull.ThenThrow();

            action(Item);
        }
    }
}
