using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypeLess;
using System;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public class StoredProcedureBuilder :
        ISprocBuilder, 
        ISprocBuilderParams, 
        ISprocBuilderComplete
    {
        private StoredProcedure _proc;

        public StoredProcedureBuilder(string connectionString, bool ownsConnection = true)
        {
            this._proc = new StoredProcedure(connectionString, null, ownsConnection);
        }

        public StoredProcedureBuilder(SqlConnection connection, bool ownsConnection = true)
        {
            this._proc = new StoredProcedure(connection, null, ownsConnection);
        }

        public StoredProcedureBuilder(SqlConnection connection, Func<SqlTransaction> transactionProvider, bool ownsConnection = true) : this(connection, ownsConnection)
        {
            this._proc.TransactionProvider = transactionProvider;
        }

        public ISprocBuilderParams WithName(string name)
        {
            name.If("name").IsNull.ThenThrow();

            if (this._proc.Parameters != null)
            {
                this._proc.Parameters.Clear();
            }

            _proc.Name = name;
            return this;
        }

        public ISprocBuilderComplete AndParameters(params Parameter[] parameters)
        {
            parameters.If("parameters").IsNull.ThenThrow();

            _proc.Parameters = new List<Parameter>(parameters);
            return this;
        }

        public IStoredProcedure Build() {
            return _proc;
        }

        public ISprocBuilderParams AndParameter(string name, object value)
        {
            name.If("name").IsNull.ThenThrow();
            _proc.AddParameter(name, value);
            return this;
        }

    }
}
