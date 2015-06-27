using System.Collections.Generic;
using System.Data;

namespace TypeLess.Net.Entity.Contracts
{
    public interface IMaterializer<T>
    {
        IList<T> ToList(IDataReader reader);
        T FirstOrDefault(IDataReader reader);
    }
}
