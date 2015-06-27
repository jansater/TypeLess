using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public class DbQuery : IDbQuery
    {
        internal DbQuery()
        {

        }

        internal string Sql { get; set; }

        internal List<Parameter> Parameters { get; set; }

        internal string ConnectionString { get; set; }

        public DbQuery(string connectionString, string sql, params Parameter[] parameters)
        {
            connectionString.If("connectionString").IsNull.ThenThrow();
            sql.If("sql").IsNull.ThenThrow();
            parameters.If("parameters").IsNull.ThenThrow();
            
            this.ConnectionString = connectionString;
            this.Sql = sql;
            Parameters = new List<Parameter>(parameters.Length);
            this.Parameters.AddRange(parameters);
        }

        internal void AddParameter(string name, object value)
        {
            name.If("name").IsNull.ThenThrow();

            if (this.Parameters == null)
            {
                this.Parameters = new List<Parameter>(5);
            }
            this.Parameters.Add(new Parameter(name, value));
        }

        public T Execute<T>(Func<DbDataReader, T> responseCallback)
        {
            responseCallback.If("responseCallback").IsNull.ThenThrow();
            //if we use the existing connection then we will get net packets out of order if two procs are called at the same time by
            //multiple threads

            //var myConnection = Connection as MySqlConnection;
            var myConnection = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(Sql, myConnection);
            
            if (this.Parameters != null)
            {
                foreach (var item in this.Parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                }
            }

            cmd.CommandType = CommandType.Text;
            myConnection.Open();
            
            T result = default(T);
            try
            {
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    result = responseCallback.Invoke(reader);
                }
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
            }
           
            return result;
        }

        public void ExecuteUpdate()
        {
            var myConnection = new SqlConnection(ConnectionString);
            var cmd = new SqlCommand(Sql, myConnection);
            if (this.Parameters != null)
            {
                foreach (var item in this.Parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                }
            }

            cmd.CommandType = CommandType.Text;

            myConnection.Open();
            try
            {
                var affectedRows = cmd.ExecuteNonQuery();
            }
            finally
            {
                myConnection.Close();
            }

        }

    }
}
