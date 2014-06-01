using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeLess.Helpers
{
    public class EnumerableTraverser 
    {

        public TState Traverse<T, TState>(IEnumerable<T> enumeration, Action<int, T, T, TState> callback, TState state, bool traverseBackwards = false) where TState : class
        {
            if (enumeration == null || callback == null || enumeration.Count() <= 0) {
                return state;
            }

            var len = enumeration.Count();

            if (len == 1) {
                callback(0, enumeration.ElementAt(0), default(T), state);
                return state;
            }

            if (!traverseBackwards)
            {
                for (int i = 0; i < len; i++)
                {
                    callback(i, enumeration.ElementAt(i), i == 0 ? default(T) : enumeration.ElementAt(i - 1), state);
                }
                return state;
            }
            else {
                for (int i = len - 1; i >= 0; i--)
                {
                    callback(i, enumeration.ElementAt(i), i == (len - 1) ? default(T) : enumeration.ElementAt(i + 1), state);
                }
                return state;
            }

        }
    }
}
