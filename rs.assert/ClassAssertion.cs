using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace TypeLess
{
    public interface IClassAssertionU<T> : IAssertionU<T> where T : class
    {
        IClassAssertion<T> Or(T obj, string withName = null);
        IClassAssertion<T> IsInvalid { get; }
        IClassAssertion<T> IsNull { get; }
        IClassAssertion<T> IsNotNull { get; }
        new IClassAssertion<T> IsTrue(Func<T, bool> assertFunc, string msgIfFalse);
        new IClassAssertion<T> IsFalse(Func<T, bool> assertFunc, string msgIfTrue);
        new IClassAssertion<T> IsNotEqualTo(T comparedTo);
        new IClassAssertion<T> IsEqualTo(T comparedTo);
        IClassAssertion<T> IsNotEqualTo<S>(S comparedTo);
        IClassAssertion<T> IsEqualTo<S>(S comparedTo);
    }

    public interface IClassAssertion<T> : IClassAssertionU<T>, IAssertion<T> where T : class
    { 
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class ClassAssertion<T> : Assertion<T>, IClassAssertion<T> where T : class
    {
     
        public ClassAssertion(string s, T source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

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

        public IClassAssertion<T> Or(T obj, string withName = null) {
            var ca = new ClassAssertion<T>(withName, obj, null, null, null);
            Add(ca);
            return this;
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
                Extend(x =>
                {
                    if (x == null)
                    {
                        var temp = StopIfNotValid;
                        return "is required";
                    }
                    return null;
                });
                return this;
            }

        }

        public IClassAssertion<T> IsNotNull
        {
            get
            {
                Extend(x =>
                {
                    if (x != null)
                    {
                        var temp = StopIfNotValid;
                        return "must be null";
                    }
                    return null;
                });
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
                Extend(x =>
                {
                    var temp = this.IsNull;

                    if (x != null)
                    {
                        dynamic d = x;
                        try
                        {
                            var inv = d.IsInvalid();
                            var classAssertions = inv as ObjectAssertion;
                            if (classAssertions != null)
                            {
                                foreach (var item in classAssertions.Assertions)
                                {
                                    Combine(item);
                                }
                            }
                        }
                        catch (RuntimeBinderException)
                        {
                            throw new System.MissingMemberException("You must define method public ObjectAssertion IsInvalid() {} in class " + typeof(T).Name);
                        }
                    }
                    return null;
                });
                return this;
            }

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


        public IClassAssertion<T> IsNotEqualTo<S>(S comparedTo)
        {
            Extend(x =>
            {
                if (x == null)
                {
                    if (comparedTo != null)
                    {
                        return "must be equal to " + comparedTo;

                    }
                    return null;
                }

                if (!x.Equals(comparedTo))
                {
                    return string.Format(CultureInfo.InvariantCulture, "must be equal to {0}", comparedTo == null ? "null" : comparedTo.ToString());
                }
                return null;
            });
            return this;
        }

        public IClassAssertion<T> IsEqualTo<S>(S comparedTo)
        {
            Extend(x =>
            {
                if (x == null)
                {
                    if (comparedTo == null)
                    {
                        return "must not be equal to " + comparedTo;
                    }
                    return null;
                }

                if (x.Equals(comparedTo))
                {
                    return string.Format(CultureInfo.InvariantCulture, "must not be equal to {0}", comparedTo == null ? "null" : comparedTo.ToString());
                }
                return null;
            });
            return this;
        }
    }
}
