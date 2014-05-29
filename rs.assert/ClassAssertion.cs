using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TypeLess
{
    public interface IClassAssertionU<T> : IAssertionU<T>
    {
        IClassAssertion<T> IsInvalid { get; }
        IClassAssertion<T> IsNull { get; }
        IClassAssertion<T> IsNotNull { get; }
        IClassAssertion<T> Or(T obj, string withName = null);
        new IClassAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse);
        new IClassAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue);
        new IClassAssertion<T> IsNotEqualTo(T comparedTo);
        new IClassAssertion<T> IsEqualTo(T comparedTo);
    }

    public interface IClassAssertion<T> : IClassAssertionU<T>, IAssertion<T>
    { 
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class ClassAssertion<T> : Assertion<T>, IClassAssertion<T> where T : class
    {
        private List<ClassAssertion<T>> _childAssertions = new List<ClassAssertion<T>>();

        public ClassAssertion(string s, T source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        internal new List<ClassAssertion<T>> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
        }

        public IClassAssertion<T> Combine(IClassAssertion<T> otherAssertion)
        {
            return (IClassAssertion<T>)base.Combine(otherAssertion);
        }

        public new IClassAssertion<T> StopIfNotValid
        {
            get {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        /// <summary>
        /// Determines whether the specified source is null. Automatically stops further processing if source is null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public IClassAssertion<T> IsNull
        {
            get
            {
                if (IgnoreFurtherChecks)
                {
                    return this;
                }

                if (Item == null)
                {
                    var x = StopIfNotValid;
                    Append("is required");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsNull);
                }

                return this;
            }

        }

        public IClassAssertion<T> IsNotNull
        {
            get
            {
                if (IgnoreFurtherChecks)
                {
                    return this;
                }

                if (Item != null)
                {
                    var x = StopIfNotValid;
                    Append("must be null");
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsNotNull);
                }

                return this;
            }

        }

        /// <summary>
        /// Make a call to this class IsValid method to determine whether the specified target object is valid. Normally used to define validation checks in for example dto's. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The target object.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
        public IClassAssertion<T> IsInvalid
        {
            get {
                if (IgnoreFurtherChecks)
                {
                    return this;
                }

                var x = this.IsNull;

                if (Item != null)
                {
                    dynamic d = Item;
                    try
                    {
                        var inv = d.IsInvalid();
                        var classAssertions = inv as IEnumerable<IAssertion>;
                        if (classAssertions != null) {
                            foreach (var item in classAssertions)
                            {
                                Combine(item);
                            }
                        }
                    }
                    catch (RuntimeBinderException)
                    {
                        throw new System.MissingMemberException("You must define method public IEnumerable<IAssertion> IsInvalid() {} in class " + typeof(T).Name);
                    }
                }

                foreach (var child in ChildAssertions)
                {
                    child.ClearErrorMsg();
                    Combine(child.IsInvalid);
                }

                return this;
            }

        }

        public IClassAssertion<T> Or(T obj, string withName = null)
        {
            this.ChildAssertions.Add(new ClassAssertion<T>(withName, obj, null, null, null));
            return this;
        }

        public new IClassAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse)
        {
            return (IClassAssertion<T>)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new IClassAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue)
        {
            return (IClassAssertion<T>)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new IClassAssertion<T> IsNotEqualTo(T comparedTo)
        {
            return (IClassAssertion<T>)base.IsNotEqualTo(comparedTo);
        }

        public new IClassAssertion<T> IsEqualTo(T comparedTo)
        {
            return (IClassAssertion<T>)base.IsEqualTo(comparedTo);
        }
    }
}
