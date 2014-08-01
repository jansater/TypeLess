using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TypeLess.DataTypes;
using TypeLess.Properties;

namespace TypeLess
{
    public interface IDictionaryAssertionU<T1, T2> : IAssertionU<IDictionary<T1, T2>>
    {
        
        IDictionaryAssertion<T1, T2> IsNull { get; }
        IDictionaryAssertion<T1, T2> IsEmpty { get; }
        IDictionaryAssertionU<T1, T2> Or(IDictionary<T1, T2> obj, string withName = null);

        new IDictionaryAssertion<T1, T2> IsTrue(Func<IDictionary<T1, T2>, bool> assertFunc, string errMsg = null);
        new IDictionaryAssertion<T1, T2> IsFalse(Func<IDictionary<T1, T2>, bool> assertFunc, string errMsg = null);

        IDictionaryAssertion<T1, T2> ContainsKey(T1 key);
        IDictionaryAssertion<T1, T2> DoesNotContainKey(T1 key);

        /// <summary>
        /// Expect statements to test validity. This effects how error messages are added. In the normal case this property is false and 
        /// assertion methods are expected to test against a negative statement such as if x is smaller than or equal to 0 then throw e.
        /// This means that the error message is added when the statement is true. This property will inverse so that error messages are added
        /// when the statement is false so when you check x == 0 then the error message is added when x is not 0
        /// </summary>
        new IDictionaryAssertionU<T1, T2> EvalPositive { get; }
    }

    public interface IDictionaryAssertion<T1, T2> : IDictionaryAssertionU<T1, T2>, IAssertion<IDictionary<T1, T2>>
    {

    }

#if !DEBUG
    [DebuggerStepThrough]
#endif
    internal class DictionaryAssertion<T1, T2> : ClassAssertion<IDictionary<T1, T2>>, IDictionaryAssertion<T1, T2>
    {
        private List<DictionaryAssertion<T1, T2>> _childAssertions = new List<DictionaryAssertion<T1, T2>>();

        public DictionaryAssertion(string s, IDictionary<T1, T2> source, string file, int? lineNumber, string caller)
            : base(s, source, file, lineNumber, caller) { }

        public IDictionaryAssertion<T1, T2> Combine(IDictionaryAssertion<T1, T2> otherAssertion)
        {
            return (IDictionaryAssertion<T1, T2>)base.Or(otherAssertion);
        }

        public new IDictionaryAssertion<T1, T2> StopIfNotValid
        {
            get
            {
                base.IgnoreFurtherChecks = true;
                return this;
            }
        }

        public new IDictionaryAssertion<T1, T2> IsNull
        {
            get
            {
                var x = base.IsNull;
                return this;
            }
        }

        public IDictionaryAssertion<T1, T2> IsEmpty
        {
            get
            {
                Extend(x => AssertResult.New(x.Count == 0, Resources.IsEmpty));
                return this;
            }
        }

        public new IDictionaryAssertionU<T1, T2> Or(IDictionary<T1, T2> obj, string withName = null)
        {
            AddWithOr(new DictionaryAssertion<T1, T2>(withName, obj, null, null, null));
            return this;
        }

        public new IDictionaryAssertion<T1, T2> IsTrue(Func<IDictionary<T1, T2>, bool> assertFunc, string errMsg = null)
        {
            return (IDictionaryAssertion<T1, T2>)base.IsTrue(assertFunc, errMsg);
        }

        public new IDictionaryAssertion<T1, T2> IsFalse(Func<IDictionary<T1, T2>, bool> assertFunc, string errMsg = null)
        {
            return (IDictionaryAssertion<T1, T2>)base.IsFalse(assertFunc, errMsg);
        }

        public IDictionaryAssertion<T1, T2> ContainsKey(T1 key)
        {
            Extend(x => {
                return AssertResult.New(x.ContainsKey(key), Resources.ContainsKey, key);
            });

            return this;
        }

        public IDictionaryAssertion<T1, T2> DoesNotContainKey(T1 key)
        {
            Extend(x =>
            {
                return AssertResult.New(!x.ContainsKey(key), Resources.DoesNotContainKey, key);
            });

            return this;
        }


        public new IDictionaryAssertionU<T1, T2> EvalPositive
        {
            get { return (IDictionaryAssertionU<T1, T2>)base.EvalPositive; }
        }
    }
}
