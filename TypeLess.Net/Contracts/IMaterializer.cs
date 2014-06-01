using System.Collections.Generic;
using System.Data;

namespace TypeLess.Net.Contracts
{
    public interface IMaterializer<T>
    {
        IList<T> ToList(IDataReader reader);
        T FirstOrDefault(IDataReader reader);
    }
}
