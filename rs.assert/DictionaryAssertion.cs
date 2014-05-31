using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TypeLess
{
    public interface IDictionaryAssertionU<T1, T2> : IAssertionU<IDictionary<T1, T2>>
    {
        IDictionaryAssertion<T1, T2> IsNull { get; }
        IDictionaryAssertion<T1, T2> IsEmpty { get; }
        IDictionaryAssertionU<T1, T2> Or(IDictionary<T1, T2> obj, string withName = null);

        new IDictionaryAssertion<T1, T2> IsTrue(Func<IDictionary<T1, T2>, bool> assertFunc, string msgIfFalse);
        new IDictionaryAssertion<T1, T2> IsFalse(Func<IDictionary<T1, T2>, bool> assertFunc, string msgIfTrue);

        IDictionaryAssertion<T1, T2> ContainsKey(T1 key);
        IDictionaryAssertion<T1, T2> DoesNotContainKey(T1 key);
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
            return (IDictionaryAssertion<T1, T2>)base.Combine(otherAssertion);
        }

        internal new List<DictionaryAssertion<T1, T2>> ChildAssertions
        {
            get
            {
                return _childAssertions;
            }
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
                Extend(x => x.Count == 0 ? "must be non empty" : null);
                return this;
            }
        }

        public IDictionaryAssertionU<T1, T2> Or(IDictionary<T1, T2> obj, string withName = null)
        {
            this.ChildAssertions.Add(new DictionaryAssertion<T1, T2>(withName, obj, null, null, null));
            return this;
        }

        public new IDictionaryAssertion<T1, T2> IsTrue(Func<IDictionary<T1, T2>, bool> assertFunc, string msgIfFalse)
        {
            return (IDictionaryAssertion<T1, T2>)base.IsTrue(assertFunc, msgIfFalse);
        }

        public new IDictionaryAssertion<T1, T2> IsFalse(Func<IDictionary<T1, T2>, bool> assertFunc, string msgIfTrue)
        {
            return (IDictionaryAssertion<T1, T2>)base.IsFalse(assertFunc, msgIfTrue);
        }

        public IDictionaryAssertion<T1, T2> ContainsKey(T1 key)
        {
            Extend(x => {
                if (x.ContainsKey(key))
                {
                    return String.Format("must not contain key {0}", key);
                }
                else {
                    return null;
                }
            });

            return this;
        }

        public IDictionaryAssertion<T1, T2> DoesNotContainKey(T1 key)
        {
            Extend(x =>
            {
                if (!x.ContainsKey(key))
                {
                    return String.Format("must not contain key {0}", key);
                }
                else
                {
                    return null;
                }
            });

            return this;
        }
    }
}
