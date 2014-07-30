﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{
    public interface INullableAssertionU<T> : IAssertionU<Nullable<T>> where T : struct
    {
        INullableAssertion<T> IsNull { get; }
        INullableAssertion<T> IsNotNull { get; }
        INullableAssertionU<T> Or(T? obj, string withName = null);

        new INullableAssertion<T> IsTrue(Func<T?, bool> assertFunc, string msgIfFalse = null);
        new INullableAssertion<T> IsFalse(Func<T?, bool> assertFunc, string msgIfTrue = null);
        new INullableAssertion<T> IsNotEqualTo(T? comparedTo);
        new INullableAssertion<T> IsEqualTo(T? comparedTo);
    }

    public interface INullableAssertion<T> : IAssertion<T?>, INullableAssertionU<T> where T : struct
    {
        
    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class NullableAssertion<T> : Assertion<Nullable<T>>, INullableAssertion<T> where T : struct
    {
        private List<NullableAssertion<T>> _childAssertions = new List<NullableAssertion<T>>();

        public NullableAssertion(string s, Nullable<T> source, string file, int? lineNumber, string caller)
            :base (s, source, file, lineNumber, caller) {}

        public INullableAssertion<T> Combine(INullableAssertion<T> otherAssertion)
        {
            return (INullableAssertion<T>)base.Or(otherAssertion);
        }

        public new NullableAssertion<T> StopIfNotValid
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
        public INullableAssertion<T> IsNull
        {
            get {
                Extend(x =>
                {
                    
                    if (x == null)
                    {
                        var temp = StopIfNotValid;
                        return AssertResult.New(true, Resources.IsNull);
                    }
                    return AssertResult.False;
                });
                return this;
            }
            
        }

        public INullableAssertion<T> IsNotNull
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
                    return AssertResult.False;
                });
                return this;
            }

        }

        public INullableAssertionU<T> Or(T? obj, string withName = null)
        {
            AddWithOr(new NullableAssertion<T>(withName, obj, null, null, null));
            return this;
        }

        public new INullableAssertion<T> IsTrue(Func<T?, bool> assertFunc, string msgIfFalse = null)
        {
            return (INullableAssertion<T>)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new INullableAssertion<T> IsFalse(Func<T?, bool> assertFunc, string msgIfTrue = null)
        {
            return (INullableAssertion<T>)base.IsFalse(assertFunc, msgIfTrue);
        }

        public new INullableAssertion<T> IsNotEqualTo(T? comparedTo)
        {
            return (INullableAssertion<T>)base.IsNotEqualTo(comparedTo);
        }

        public new INullableAssertion<T> IsEqualTo(T? comparedTo)
        {
            return (INullableAssertion<T>)base.IsEqualTo(comparedTo);
        }
    }
}
