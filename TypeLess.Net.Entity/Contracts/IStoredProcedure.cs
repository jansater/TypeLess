using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace TypeLess.Net.Entity.Contracts
{
    public interface ISprocBuilder
    {
        ISprocBuilderParams WithName(string name);
    }

    public interface ISprocBuilderParams
    {
        ISprocBuilderComplete AndParameters(params Parameter[] parameters);
        ISprocBuilderParams AndParameter(string name, object value);
        IStoredProcedure Build();
    }

    public interface ISprocBuilderComplete
    {
        IStoredProcedure Build();
    }

    public interface IStoredProcedure
    {
        T Execute<T>(Func<DbDataReader, T> materializeCallback);
        void ExecuteUpdate();
    }
}
