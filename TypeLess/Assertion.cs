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
        /// <summary>
        /// Add a new assertion of a different type. Returns a mixed type assertion with only a few possible checks available.
        /// </summary>
        IMixedTypeAssertionU<T, S> Or<S>(S obj, string withName = null) where S : class;

        /// <summary>
        /// Checks wether the given statement is true
        /// </summary>
        /// <param name="assertFunc"></param>
        /// <param name="msgIfFalse">Message to return. Use <name> to include the parameter name in the string.</param>
        /// <returns></returns>
        IAssertion<T> IsTrue(Func<T, bool> assertFunc, string errMsg = null);
        /// <summary>
        /// Checks wether the given statement is false
        /// </summary>
        /// <param name="assertFunc"></param>
        /// <param name="msgIfFalse">Message to return. Use <name> to include the parameter name in the string.</param>
        /// <returns></returns>
        IAssertion<T> IsFalse(Func<T, bool> assertFunc, string errMsg = null);

        /// <summary>
        /// Checks whether the current Item is not equal to comparedTo
        /// </summary>
        IAssertion<T> IsNotEqualTo(T comparedTo);

        /// <summary>
        /// Checks whether the current Item is equal to comparedTo
        /// </summary>
        IAssertion<T> IsEqualTo(T comparedTo);

        /// <summary>
        /// Add the assertFunc to the validation chain
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Extend(Func<T, AssertResult> assertFunc);

        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        IAssertionU<T> EvalPositive { get; }
    }

    public interface IAssertion : IAssertionU
    {
        /// <summary>
        /// Number of errors generated
        /// </summary>
        int ErrorCount { get; }
        /// <summary>
        /// Skip checking further assertions
        /// </summary>
        bool IgnoreFurtherChecks { get; }
        /// <summary>
        /// Return the current error message
        /// </summary>
        /// <param name="skipTrace">if set to <c>true</c> [skip trace].</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        string ToString(bool skipTrace);
        /// <summary>
        /// Return the current error message
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        string ToString();
        /// <summary>
        /// Add otherAssertion to the validation chain, use separator between
        /// the two assertion error messages
        /// </summary>
        IAssertion Or(IAssertion otherAssertion, string separator = ". ");
        /// <summary>
        /// Returns the property name
        /// </summary>
        string Name { get; }
        
    }

    public interface IAssertionOW<T>
    {
        /// <summary>
        /// If no exception thrown, call action
        /// </summary>
        /// <param name="action">The action to run</param>
        void Otherwise(Action<T> action);

        /// <summary>
        /// If no exception thrown, call action
        /// </summary>
        /// <param name="action">The action to run</param>
        S OtherwiseReturn<S>(Func<T, S> func);

        /// <summary>
        /// If the current validation chain is true then return the value from func
        /// </summary>
        /// <param name="func">The function to call, T is the current item. S is the type of value to return</param>
        S ThenReturn<S>(Func<T, S> func);

        /// <summary>
        /// If the current validation chain is true then return the value from func
        /// </summary>
        /// <param name="func">The function to call, T is the current item. S is the type of value to return</param>
        S ThenReturn<S>(Func<S> func);

        /// <summary>
        /// If the current validation chain is true then return the value from func
        /// </summary>
        /// <param name="func">The function to call, T is the current item. S is the type of value to return</param>
        S ThenReturn<S>(S valueToReturn);

        /// <summary>
        /// Throws an argument null exception.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow(string errorMsg = null, params object[] args);
        /// <summary>
        /// Throws an exception of type T.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow<E>(string errorMsg = null, params object[] args) where E : Exception;

        /// <summary>
        /// Throws an argument null exception.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow(Exception innerException, string errorMsg = null, params object[] args);
        /// <summary>
        /// Throws an exception of type T.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow<E>(Exception innerException, string errorMsg = null, params object[] args) where E : Exception;

    }

    public interface IAssertion<T> : IAssertion, IAssertionU<T>
    {
        /// <summary>
        /// Add otherAssertion to the validation chain, use separator between
        /// the two assertion error messages
        /// </summary>
        IAssertion<T> Or<S>(IAssertion<S> otherAssertion, string separator = ". ");

        /// <summary>
        /// If the current validation chain is true then call action
        /// </summary>
        /// <param name="action">The method to call, T is the current Item</param>
        IAssertionOW<T> Then(Action<T> action);
        /// <summary>
        /// If the current validation chain is true then return the value from func
        /// </summary>
        /// <param name="func">The function to call, T is the current item. S is the type of value to return</param>
        S ThenReturn<S>(Func<T, S> func);

        /// <summary>
        /// If the current validation chain is true then return the value from func
        /// </summary>
        /// <param name="func">The function to call, T is the current item. S is the type of value to return</param>
        S ThenReturn<S>(Func<S> func);

        /// <summary>
        /// If the current validation chain is true then return the value from func
        /// </summary>
        /// <param name="func">The function to call, T is the current item. S is the type of value to return</param>
        S ThenReturn<S>(S valueToReturn);

        /// <summary>
        /// Logs all details of the method such as line number and file
        /// </summary>
        /// <param name="logMethod"></param>
        /// <returns></returns>
        IAssertion<T> ThenLogTo(Action<string> logMethod);
        /// <summary>
        /// Logs all details of the method such as line number and file
        /// </summary>
        /// <param name="logMethod"></param>
        /// <returns></returns>
        IAssertion<T> ThenLogTo(string customMsg, Action<string> logMethod);

        /// <summary>
        /// Sets the error message to s. Useful for object assertions
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        IAssertion<T> ThenSetErrorMessage(string s);

        /// <summary>
        /// Throws an argument null exception.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow(string errorMsg = null, params object[] args);
        /// <summary>
        /// Throws an exception of type T.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow<E>(string errorMsg = null, params object[] args) where E : Exception;

        /// <summary>
        /// Throws an argument null exception.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow(Exception innerException, string errorMsg = null, params object[] args);
        /// <summary>
        /// Throws an exception of type T.
        /// </summary>
        /// <param name="errorMsg">Override the system generated message with your own. Use <name> to include the paramater name in the message</param>
        /// <returns></returns>
        IAssertionOW<T> ThenThrow<E>(Exception innerException, string errorMsg = null, params object[] args) where E : Exception;

        /// <summary>
        /// Tries to execute the specified try action.
        /// </summary>
        /// <param name="tryAction">The try action.</param>
        /// <param name="catchAction">if the try fails catch action is called with the exception</param>
        /// <param name="finallyAction">if set the finally action will always be called when the try/catch has run</param>
        void Try(Action<T> tryAction, Action<Exception> catchAction, Action<T> finallyAction = null);

    }


#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class Assertion<T> : TypeLess.IAssertion<T>, IAssertion, IAssertionOW<T>
    {
        protected internal StringBuilder _sb { get; set; }
        internal T Item { get; set; }

        public string Name { get; internal set; }
        protected bool _isValid = false;
        private string _file;
        private int? _lineNr;
        private string _caller;
        protected List<Assertion<T>> _children = new List<Assertion<T>>();
        protected int _errorCount;
        protected bool _evalPositive = false;
        private string _andFailure = "";

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

        public IAssertionU<T> EvalPositive
        {
            get
            {
                _isValid = true;
                _evalPositive = true;
                return this;
            }
        }

        /// <summary>
        /// Adds a child assertion as or
        /// </summary>
        /// <param name="assertion"></param>
        internal void AddWithOr(Assertion<T> assertion)
        {
            if (assertion == null)
            {
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

        private static string ExtractDetails(StringBuilder sb, Exception e)
        {
            if (!String.IsNullOrEmpty(e.Message))
            {
                sb.Append(e.Message);
            }

            int i = 1;
            while (e.InnerException != null)
            {
                sb.AppendLine("INNER EXCEPTION ").Append(i).Append(":").AppendLine();
                i++;
                sb.Append(e.InnerException.StackTrace);
                e = e.InnerException;
            }
            return sb.ToString();
        }

        public IAssertionOW<T> ThenThrow<E>(Exception innerException, string errorMsg = null, params object[] args) where E : Exception
        {
            if (!True)
            {
                return this;
            }

            if (errorMsg != null && args != null && args.Length > 0)
            {
                errorMsg = String.Format(CultureInfo.InvariantCulture, errorMsg, args);
            }
            
            if (Debugger.IsAttached)
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg == null ? ToString() : AppendTrace(String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name)), innerException });

            }
            else
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg == null ? ToString() : String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name), innerException });
            }
        }

        public IAssertion<T> ThenLogTo(Action<string> logMethod) {
            if (logMethod == null)
            {
                throw new ArgumentNullException("logMethod");
            }

            var toLog = AppendTrace(ToString(skipTrace: true));
            logMethod(toLog);
            return this;
        }

        public IAssertion<T> ThenLogTo(string customMsg, Action<string> logMethod) {
            if (logMethod == null) {
                throw new ArgumentNullException("logMethod");
            }

            var toLog = customMsg == null ? ToString(skipTrace:true) : String.Format(CultureInfo.InvariantCulture, customMsg.Replace("<name>", "{0}"), Name);
            toLog = AppendTrace(toLog);

            logMethod(toLog);
            return this;
        }

        public IAssertionOW<T> ThenThrow<E>(string errorMsg = null, params object[] args) where E : Exception
        {
            if (!True)
            {
                return this;
            }

            if (errorMsg != null && args != null && args.Length > 0)
            {
                errorMsg = String.Format(CultureInfo.InvariantCulture, errorMsg, args);
            }

            if (Debugger.IsAttached)
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg == null ? ToString() : AppendTrace(String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name)) });

            }
            else
            {
                throw (Exception)Activator.CreateInstance(typeof(E), new object[] { errorMsg == null ? ToString() : String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name) });
            }

        }


        /// <summary>
        /// Throws arg null instead of arg exception just because of the message created
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IAssertionOW<T> ThenThrow(Exception innerException, string errorMsg = null, params object[] args)
        {
            if (!True)
            {
                return this;
            }

            if (errorMsg != null && args != null && args.Length > 0)
            {
                errorMsg = String.Format(CultureInfo.InvariantCulture, errorMsg, args);
            }

            if (Debugger.IsAttached)
            {
                throw new ArgumentNullException(errorMsg == null ? ToString() : AppendTrace(
                    String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name)), innerException);
            }
            else
            {
                throw new ArgumentNullException(errorMsg == null ?
                   ToString() :
                    String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name), innerException);
            }
        }

        /// <summary>
        /// Throws arg null instead of arg exception just because of the message created
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IAssertionOW<T> ThenThrow(string errorMsg = null, params object[] args)
        {
            if (!True)
            {
                return this;
            }

            if (errorMsg != null && args != null && args.Length > 0)
            {
                errorMsg = String.Format(CultureInfo.InvariantCulture, errorMsg, args);
            }

            if (Debugger.IsAttached)
            {
                throw new ArgumentNullException("", errorMsg == null ? ToString() : AppendTrace(
                    String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name)));
            }
            else
            {
                throw new ArgumentNullException("", errorMsg == null ?
                   ToString() :
                    String.Format(CultureInfo.InvariantCulture, errorMsg.Replace("<name>", "{0}"), Name));
            }
        }

        public IAssertion<T> ThenSetErrorMessage(string s) {
            _sb.Clear();
            _sb.Append(s);

            return this;
        }

        public override string ToString()
        {
            return this.ToString(skipTrace: false);
        }

        public string ToString(bool skipTrace)
        {
            if ((!True && !_evalPositive) || (True && _evalPositive))
            {
                return String.Empty;
            }

            var errMessage = _sb.ToString();

            if (!String.IsNullOrEmpty(_andFailure))
            {
                errMessage = ConvertErrorMessageToAndErrorMessage(errMessage);
                errMessage = _andFailure + errMessage;
            }

            if (Debugger.IsAttached && !skipTrace)
            {
                return AppendTrace(String.Format(CultureInfo.InvariantCulture, errMessage.Replace("<name>", "{0}"), Name));
            }
            else
            {
                return String.Format(CultureInfo.InvariantCulture, errMessage.Replace("<name>", "{0}"), Name);
            }
        }

        private string ConvertErrorMessageToAndErrorMessage(string errMessage)
        {
            errMessage = errMessage.Replace("must contain more than", "contain less than");
            errMessage = errMessage.Replace("must contain less than", "contain more than");
            errMessage = errMessage.Replace("must end with", "does not end with");
            errMessage = errMessage.Replace("must match", "does not match");
            errMessage = errMessage.Replace("must not match", "match");
            errMessage = errMessage.Replace("must start with", "does not start with");
            errMessage = errMessage.Replace("must be smaller than", "is larger than");
            errMessage = errMessage.Replace("must be larger than", "is smaller than");
            errMessage = errMessage.Replace("must not be empty", "is empty");
            errMessage = errMessage.Replace("must not be", "is");
            errMessage = errMessage.Replace("must be", "is not");
            errMessage = errMessage.Replace("must not contain", "contain");
            errMessage = errMessage.Replace("must contain", "does not contain");
            errMessage = errMessage.Replace("is required", "is not required");
            errMessage = errMessage.Replace("  ", " ");
            errMessage = errMessage.Trim();

            return errMessage;
        }

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

        internal void MakePartOfAndOperation(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                //Then we did not have a failure on the previous operator which means 
                //that the andOperation will succeed ... we don't have to check any further
                IgnoreFurtherChecks = true;
                return;
            }

            if (s.Contains(" when "))
            {
                _andFailure = s + " and ";
            }
            else
            {
                _andFailure = s + " when ";
            }
        }

        internal void Append(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
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

        public IAssertion<T> IsTrue(Func<T, bool> assertFunc, string errMsg)
        {
            return AssertExtensions.IsTrue(this, assertFunc, errMsg);
        }

        public IAssertion<T> IsFalse(Func<T, bool> assertFunc, string errMsg)
        {
            return AssertExtensions.IsFalse(this, assertFunc, errMsg);
        }

        public IAssertion<T> IsNotEqualTo(T comparedTo)
        {
            return AssertExtensions.IsNotEqualTo(this, comparedTo);
        }

        public IAssertion<T> IsEqualTo(T comparedTo)
        {
            return AssertExtensions.IsEqualTo(this, comparedTo);
        }

        public IAssertionOW<T> Then(Action<T> action)
        {
            action.If().IsNull.ThenThrow();

            if (!True)
            {
                return this;
            }
            action(Item);
            return this;
        }

        public S ThenReturn<S>(Func<T, S> func)
        {
            if (!True)
            {
                return default(S);
            }

            return func(Item);
        }

        public void Extend(Func<T, AssertResult> assertFunc)
        {
            if (IgnoreFurtherChecks)
            {
                return;
            }

            var s = assertFunc(Item);

            if (s != null && s.Message != null && ((!_evalPositive && s.IsValid) || (_evalPositive && !s.IsValid)))
            {
                Append(s.Message);
            }

            if (!_evalPositive)
            {
                //default
                _isValid |= s.IsValid;
            }
            else
            {
                _isValid &= s.IsValid;
            }

            foreach (var child in _children)
            {
                if (child is Assertion<T>)
                {
                    var childAssertion = (Assertion<T>)child;
                    var res = assertFunc(childAssertion.Item);

                    if (res != null && res.Message != null && ((!_evalPositive && res.IsValid) || (_evalPositive && !res.IsValid)))
                    {
                        _errorCount++;
                        if (_sb.Length <= 0)
                        {
                            _sb.Append(String.Format(CultureInfo.InvariantCulture, res.Message.Replace("<name>", "{0}"), childAssertion.Name));
                        }
                        else
                        {
                            _sb.Append(". ").Append(String.Format(CultureInfo.InvariantCulture, res.Message.Replace("<name>", "{0}"), childAssertion.Name));
                        }

                        if (!_evalPositive)
                        {
                            //default
                            _isValid |= res.IsValid;
                        }
                        else
                        {
                            _isValid &= res.IsValid;
                        }

                    }
                    else if (res != null && !res.IsValid)
                    {
                        _isValid |= false;
                    }
                }
            }

            if (_errorCount <= 0)
            {
                _sb.Clear();
            }
        }

        public void Otherwise(Action<T> action)
        {
            action.If("action").IsNull.ThenThrow();

            action(Item);
        }


        public void Try(Action<T> tryAction, Action<Exception> catchAction, Action<T> finallyAction = null)
        {
            tryAction.If("tryAction")
                .Or(catchAction, "catchAction")
                .IsNull.ThenThrow();

            try
            {
                tryAction(Item);
            }
            catch (Exception ex)
            {
                catchAction(ex);
            }
            finally
            {
                if (finallyAction != null)
                {
                    finallyAction(Item);
                }
            }
        }


        public S ThenReturn<S>(Func<S> func)
        {
            if (!True)
            {
                return default(S);
            }

            return func();
        }


        public S ThenReturn<S>(S valueToReturn)
        {
            if (!True)
            {
                return default(S);
            }

            return valueToReturn;
        }


        public S OtherwiseReturn<S>(Func<T, S> func)
        {
            func.If("func").IsNull.ThenThrow();

            return func(Item);
        }
    }
}
