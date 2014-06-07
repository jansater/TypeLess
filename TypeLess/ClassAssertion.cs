using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TypeLess.DataTypes;

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
            AddWithOr(ca);
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
                        return AssertResult.New(true, "<name> is required");
                    }
                    return AssertResult.New(false);
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
                        return AssertResult.New(true, "<name> must be null");
                    }
                    return AssertResult.New(false);
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
                    if (x != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        int errCount = 0;
                        dynamic d = x;
                        try
                        {
                            var inv = d.IsInvalid();
                            var classAssertions = inv as ObjectAssertion;
                            if (classAssertions != null)
                            {
                                foreach (var item in classAssertions.Assertions)
                                {
                                    var s = item.ToString();

                                    errCount += item.ErrorCount;
                                    sb.Append(s);
                                }
                            }
                        }
                        catch (RuntimeBinderException)
                        {
                            throw new System.MissingMemberException("You must define method public ObjectAssertion IsInvalid() {} in class " + typeof(T).Name);
                        }

                        return AssertResult.New(errCount > 0, sb.ToString());
                    }
                    else {
                        return AssertResult.New(x.If().IsNull.True, Name + " must not be null");
                    }
                    
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
                    return AssertResult.New(comparedTo != null, "<name> must be equal to " + comparedTo);
                }

                return AssertResult.New(!x.Equals(comparedTo), "<name> must be equal to {0}", comparedTo == null ? "null" : comparedTo.ToString());
            });
            return this;
        }

        public IClassAssertion<T> IsEqualTo<S>(S comparedTo)
        {
            Extend(x =>
            {
                if (x == null)
                {
                    return AssertResult.New(comparedTo == null, "<name> must not be equal to " + comparedTo);
                }

                return AssertResult.New(x.Equals(comparedTo), "<name> must not be equal to {0}", comparedTo == null ? "null" : comparedTo.ToString());
            });
            return this;
        }
    }
}
