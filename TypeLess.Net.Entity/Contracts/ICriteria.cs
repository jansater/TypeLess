using System.Linq;

namespace TypeLess.Net.Entity.Contracts
{
    public interface ICriteria<T> where T : class
    {
        IQueryable<T> GetMatches(IQueryable<T> queryBase);

    }
}
