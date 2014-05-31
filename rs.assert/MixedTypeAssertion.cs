using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace TypeLess
{
    public interface IMixedTypeAssertionU<T, U> : IAssertionU<T> 
        where T : class 
        where U : class
    {
        IClassAssertion<T> IsInvalid { get; }
        IClassAssertion<T> IsNull { get; }
        IClassAssertion<T> IsNotNull { get; }
        IClassAssertion<T> IsNotEqualTo<S>(S comparedTo);
        IClassAssertion<T> IsEqualTo<S>(S comparedTo);
    }

    public interface IMixedTypeAssertion<T, U> : IMixedTypeAssertionU<T, U>, IAssertion<T>
        where T : class
        where U : class
    { 
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class MixedTypeAssertion<T, U> : Assertion<T>, IMixedTypeAssertion<T, U>
        where T : class
        where U : class
    {
        public MixedTypeAssertion(string s, T source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public IMixedTypeAssertion<T, U> Combine(IMixedTypeAssertion<T, U> otherAssertion)
        {
            return (IMixedTypeAssertion<T, U>)base.Combine(otherAssertion);
        }

        public new IMixedTypeAssertion<T, U> StopIfNotValid
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
        public IMixedTypeAssertion<T, U> IsNull
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

        public IMixedTypeAssertion<T, U> IsNotNull
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
        public IMixedTypeAssertion<T, U> IsInvalid
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

        public IMixedTypeAssertion<T, U> IsNotEqualTo<S>(S comparedTo)
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

        public IMixedTypeAssertion<T, U> IsEqualTo<S>(S comparedTo)
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
