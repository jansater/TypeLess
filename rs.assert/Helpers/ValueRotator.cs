using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess;

namespace TypeLess.Helpers
{
    /// <summary>
    /// Rotates between the given values on each call in order
    /// </summary>
    public class ValueRotator<T>
    {
        private int _index = 0;
        private T[] _arr;

        public ValueRotator(params T[] arr)
        {
            arr.If("arr").IsNull.ThenThrow();

            _arr = arr;
        }

        public void Reset()
        {
            _index = 0;
        }

        /// <summary>
        /// Get the next value
        /// </summary>
        /// <returns></returns>
        public T Get()
        {
            var val =  _arr[_index++];
            if (_index > _arr.Length - 1) {
                _index = 0;
            }
            return val;
        }
    }
}
