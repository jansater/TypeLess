using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using TypeLess.Net.Contracts;

namespace TypeLess.Net.Data
{
    public class StoredProcedure : IStoredProcedure
    {
        internal StoredProcedure()
        {

        }

        internal string Name { get; set; }

        internal List<Parameter> Parameters { get; set; }

        internal string ConnectionString { get; set; }

        public StoredProcedure(string connectionString, string procedureName, params Parameter[] parameters)
        {
            this.ConnectionString = connectionString;
            this.Name = procedureName;
            Parameters = new List<Parameter>(parameters.Length);
            this.Parameters.AddRange(parameters);
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
            
            //if we use the existing connection then we will get net packets out of order if two procs are called at the same time by
            //multiple threads

            //var myConnection = Connection as MySqlConnection;
            var myConnection = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand(Name, myConnection);
            if (this.Parameters != null)
            {
                foreach (var item in this.Parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                }
            }

            cmd.CommandType = CommandType.StoredProcedure;

            myConnection.Open();
            //var command = myConnection.CreateCommand();
            ////command.CommandText = "SET NAMES 'utf8';SET CHARACTER SET 'UTF8';SET COLLATION_CONNECTION='utf8_general_ci';";  
            //command.CommandText = " SET SESSION optimizer_search_depth = 0;";  
            //command.CommandType = CommandType.Text;
            //int x = command.ExecuteNonQuery();

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
            var cmd = new SqlCommand(Name, myConnection);
            if (this.Parameters != null)
            {
                foreach (var item in this.Parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(item.Name, item.Value));
                }
            }

            cmd.CommandType = CommandType.StoredProcedure;

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
