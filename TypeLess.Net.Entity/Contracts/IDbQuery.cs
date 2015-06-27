using System;
using System.Data;
using System.Data.Common;

namespace TypeLess.Net.Entity.Contracts
{
    public interface IDbQueryBuilder
    {
        IDbQueryBuilderParams WithSql(string sql);
    }

    public interface IDbQueryBuilderParams
    {
        IDbQueryBuilderComplete AndParameters(params Parameter[] parameters);
        IDbQueryBuilderParams AndParameter(string name, object value);
        IDbQuery Build();
    }

    public interface IDbQueryBuilderComplete
    {
        IDbQuery Build();
    }

    public interface IDbQuery
    {
        T Execute<T>(Func<DbDataReader, T> materializeCallback);
        void ExecuteUpdate();
    }
}
