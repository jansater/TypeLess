using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypeLess;
using System;
using TypeLess.Net.Entity.Contracts;
using System.Linq;

namespace TypeLess.Net.Entity
{
    public class StoredProcedureBuilder :
        ISprocBuilder, 
        ISprocBuilderParams, 
        ISprocBuilderComplete
    {

        private string _connectionString;
        private Func<IDbConnection> _connectionProvider;
        private Func<IDbTransaction> _transactionProvider;
        private string _name;
        private List<Parameter> _parameters = new List<Parameter>();
        
        public StoredProcedureBuilder(string connectionString, Func<IDbTransaction> transactionProvider = null)
        {
            connectionString.If("connectionString").IsNull.ThenThrow().Otherwise(x => _connectionString = x);
            _transactionProvider = transactionProvider ?? (() => null);
        }

        public StoredProcedureBuilder(Func<IDbConnection> connectionProvider, Func<IDbTransaction> transactionProvider = null)
        {
            connectionProvider.If("connectionProvider").IsNull.ThenThrow().Otherwise(x => _connectionProvider = x);
            _transactionProvider = transactionProvider ?? (() => null);
        }

        public ISprocBuilderParams WithName(string name)
        {
            name.If("name").IsNull.ThenThrow();
            _name = name;
            return this;
        }

        public ISprocBuilderComplete AndParameters(params Parameter[] parameters)
        {
            parameters.If("parameters").IsNull.ThenThrow();
            _parameters.AddRange(parameters);
            return this;
        }

        public IStoredProcedure Build() {
            StoredProcedure proc;

            var transaction = _transactionProvider();

            if (transaction != null)
            {
                proc = new StoredProcedure(transaction.Connection, transaction, _name, _parameters.ToArray());
            }
            else if (_connectionProvider != null)
            {
                proc = new StoredProcedure(_connectionProvider(), _name, _parameters.ToArray());
            }
            else
            {
                proc = new StoredProcedure(_connectionString, _name, _parameters.ToArray());
            }
            
            Reset();

            return proc;
        }

        private void Reset() {
            _parameters.Clear();
            _name = null;
        }

        public ISprocBuilderParams AndParameter(string name, object value)
        {
            name.If("name").IsNull.ThenThrow();
            _parameters.Add(new Parameter(name, value));
            return this;
        }

    }
}
