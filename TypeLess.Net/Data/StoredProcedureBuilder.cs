using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TypeLess.Net.Contracts;
using TypeLess;

namespace TypeLess.Net.Data
{
    public class StoredProcedureBuilder :
        ISprocBuilder, 
        ISprocBuilderParams, 
        ISprocBuilderComplete
    {
        private StoredProcedure _proc;

        public StoredProcedureBuilder(string connectionString)
        {
            this._proc = new StoredProcedure();
            this._proc.ConnectionString = connectionString;
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
