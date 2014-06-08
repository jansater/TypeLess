using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{
    public interface IMixedTypeAssertionU<T, U> : IAssertionU<T> 
        where U : class
    {
        IMixedTypeAssertion<T, U> IsInvalid { get; }
        IMixedTypeAssertion<T, U> IsNull { get; }
        IMixedTypeAssertion<T, U> IsNotNull { get; }
        IMixedTypeAssertion<T, U> IsNotEqualTo<S>(S comparedTo);
        IMixedTypeAssertion<T, U> IsEqualTo<S>(S comparedTo);
    }

    public interface IMixedTypeAssertion<T, U> : IMixedTypeAssertionU<T, U>, IAssertion<T>
        where U : class
    { 
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class MixedTypeAssertion<T, U> : Assertion<T>, IMixedTypeAssertion<T, U>
        where U : class
    {
        private U _newType = null;

        public MixedTypeAssertion(string s, T source, U source2)
            :base (s, source, null, null, null) {
                _newType = source2;
        }

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
                        return AssertResult.New(true, Resources.IsNull);
                    }
                    return AssertResult.New(false);
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
                        return AssertResult.New(true, Resources.IsNotNull);
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
                    return AssertResult.New(comparedTo != null, Resources.IsNotEqualTo, comparedTo);
                }

                return AssertResult.New(!x.Equals(comparedTo), Resources.IsNotEqualTo, comparedTo == null ? "null" : comparedTo.ToString());
            });
            return this;
        }

        public IMixedTypeAssertion<T, U> IsEqualTo<S>(S comparedTo)
        {
            Extend(x =>
            {
                if (x == null)
                {
                    return AssertResult.New(comparedTo == null, Resources.IsEqualTo, comparedTo);
                }

                return AssertResult.New(x.Equals(comparedTo), Resources.IsEqualTo, comparedTo == null ? "null" : comparedTo.ToString());
            });
            return this;
        }

    }
}
