
using System;
namespace TypeLess.Net.Data.Materializers
{
    public class DynamicMaterializer<T> : BaseMaterializer<T> where T : class
    {
        private Func<System.Data.IDataReader, T> _func;

        public DynamicMaterializer(int expectedSize, Func<System.Data.IDataReader, T> func)
            : base(expectedSize)
        {
            _func = func;
        }

        protected override T ToEntity(System.Data.IDataReader reader)
        {
           return _func(reader);
        }

        
    }
}
