using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TypeLess;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public class StoredProcedure : IStoredProcedure
    {
        private string _name;
        private List<Parameter> _parameters = new List<Parameter>();
        private string _connectionString;
        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public StoredProcedure(string connectionString, string procedureName = null, params Parameter[] parameters) 
        {
            _name = procedureName;
            _connectionString = connectionString;

            if (parameters != null)
            {
                _parameters = new List<Parameter>(parameters);
            }
        }

        public StoredProcedure(IDbConnection existingConnection, string procedureName = null, params Parameter[] parameters)
        {
            existingConnection.If("existingConnection").IsNull.ThenThrow().Otherwise(x => _connection = x);
            _name = procedureName;

            if (parameters != null)
            {
                _parameters = new List<Parameter>(parameters);
            }
        }

        public StoredProcedure(IDbConnection existingConnection, IDbTransaction transaction, string procedureName = null, params Parameter[] parameters)
        {
            existingConnection.If("existingConnection").IsNull.ThenThrow().Otherwise(x => _connection = x);
            transaction.If("transaction").IsNull.ThenThrow().Otherwise(x => _transaction = x);

            _name = procedureName;

            if (parameters != null)
            {
                _parameters = new List<Parameter>(parameters);
            }
        }

        public T Execute<T>(Func<DbDataReader, T> responseCallback)
        {
            responseCallback.If("responseCallback").IsNull.ThenThrow();
            return ExecuteWithConnection(cmd =>
                {
                    T result = default(T);
                    using (var reader = cmd.ExecuteReader(_connection == null ? CommandBehavior.CloseConnection : CommandBehavior.Default))
                    {
                        result = responseCallback.Invoke(reader);
                    }
                    return result;
                });
        }

        private T ExecuteWithConnection<T>(Func<SqlCommand, T> func) {

            _name.If("Name").IsNull.ThenThrow();

            bool ownsConnection = _connection == null;

            if (ownsConnection)
            {
                _connection = new SqlConnection(_connectionString);
            }

            var cmd = new SqlCommand(_name, _connection as SqlConnection);
            
            if (this._parameters != null)
            {
                foreach (var item in _parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                }
            }

            cmd.CommandType = CommandType.StoredProcedure;

            if (ownsConnection)
            {
                _connection.Open();
            }

            if (_transaction != null)
            {
                cmd.Transaction = _transaction as SqlTransaction;
            }

            try
            {
                return func(cmd);
            }
            finally
            {
                if (ownsConnection)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }

        public void ExecuteUpdate()
        {
            ExecuteWithConnection(cmd => cmd.ExecuteNonQuery() );
        }

    }
}
