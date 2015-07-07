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
        internal StoredProcedure()
        {

        }

        internal string Name { get; set; }

        internal List<Parameter> Parameters { get; set; }

        private string ConnectionString { get; set; }

        private SqlConnection Connection { get; set; }

        private bool _ownsConnection = false;

        public StoredProcedure(string connectionString, string procedureName = null, bool ownsConnection = true, params Parameter[] parameters) 
        {
            Name = procedureName;
            _ownsConnection = ownsConnection;
            ConnectionString = connectionString;

            if (parameters != null)
            {
                Parameters = new List<Parameter>(parameters.Length);
                this.Parameters.AddRange(parameters);
            }
        }

        public StoredProcedure(SqlConnection existingConnection, string procedureName = null, bool ownsConnection = true, params Parameter[] parameters) {
            existingConnection.If("existingConnection").IsNull.ThenThrow().Otherwise(x => Connection = x);
            Name = procedureName;
            _ownsConnection = ownsConnection;

            if (parameters != null) {
                Parameters = new List<Parameter>(parameters.Length);
                this.Parameters.AddRange(parameters);
            }
        }

        internal void AddParameter(string name, object value)
        {
            if (this.Parameters == null)
            {
                this.Parameters = new List<Parameter>(5);
            }
            this.Parameters.Add(new Parameter(name, value));
        }

        public T Execute<T>(Func<DbDataReader, T> responseCallback)
        {
            responseCallback.If("responseCallback").IsNull.ThenThrow();
            return ExecuteWithConnection(cmd =>
                {
                    T result = default(T);
                    using (var reader = cmd.ExecuteReader(_ownsConnection ? CommandBehavior.CloseConnection : CommandBehavior.Default))
                    {
                        result = responseCallback.Invoke(reader);
                    }
                    return result;
                });
        }

        private T ExecuteWithConnection<T>(Func<SqlCommand, T> func) {

            Name.If("Name").IsNull.ThenThrow();

            SqlTransaction transaction = null;
            if (TransactionProvider != null)
            {
                transaction = TransactionProvider();
            }

            if (Connection == null) {
                if (transaction != null)
                {
                    _ownsConnection = false;
                    Connection = transaction.Connection;
                }
                else {
                    _ownsConnection = true;
                    Connection = new SqlConnection(ConnectionString);
                }
            }

            var cmd = new SqlCommand(Name, Connection);
            
            if (this.Parameters != null)
            {
                foreach (var item in this.Parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                }
            }

            cmd.CommandType = CommandType.StoredProcedure;

            if (_ownsConnection)
            {
                Connection.Open();
            }

            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }

            try
            {
                return func(cmd);
            }
            finally
            {
                if (_ownsConnection)
                {
                    Connection.Dispose();
                    Connection = null;
                }
            }
        }

        public void ExecuteUpdate()
        {
            ExecuteWithConnection(cmd => cmd.ExecuteNonQuery() );
        }

        public Func<SqlTransaction> TransactionProvider { get; set; }
    }
}
