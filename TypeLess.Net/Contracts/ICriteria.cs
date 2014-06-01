using System.Linq;

namespace TypeLess.Net.Contracts
{
    public interface ICriteria<T> where T : class
    {
        IQueryable<T> GetMatches(IQueryable<T> queryBase);

    }
}
