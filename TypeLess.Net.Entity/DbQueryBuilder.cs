using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypeLess.Net.Entity.Contracts;


namespace TypeLess.Net.Entity
{
    public class DbQueryBuilder : 
        IDbQueryBuilder, 
        IDbQueryBuilderParams, 
        IDbQueryBuilderComplete
    {
        private DbQuery _query;

        public DbQueryBuilder(string connectionString)
        {
            this._query = new DbQuery();
            this._query.ConnectionString = connectionString;
        }

        public IDbQueryBuilderParams WithSql(string sql)
        {
            sql.If("sql").IsNull.ThenThrow();

            if (this._query.Parameters != null)
            {
                this._query.Parameters.Clear();
            }

            _query.Sql = sql;
            return this;
        }

        public IDbQueryBuilderComplete AndParameters(params Parameter[] parameters)
        {
            parameters.If("parameters").IsNull.ThenThrow();
            _query.Parameters = new List<Parameter>(parameters);
            return this;
        }

        public IDbQuery Build()
        {
            return _query;
        }

        public IDbQueryBuilderParams AndParameter(string name, object value)
        {
            name.If("name").IsNull.ThenThrow();
            _query.AddParameter(name, value);
            return this;
        }
    }
}
