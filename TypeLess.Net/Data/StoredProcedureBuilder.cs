﻿using System.Collections.Generic;
using System.Data;
using TypeLess.Net.Contracts;


namespace TypeLess.Net.Data
{
    public class StoredProcedureBuilder : 
        ISprocBuilder, 
        ISprocBuilderParams, 
        ISprocBuilderComplete
    {
        private StoredProcedure _proc;

        public StoredProcedureBuilder(IDbConnection connection)
        {
            _proc = new StoredProcedure();
            _proc.Connection = connection;
        }

        public ISprocBuilderParams WithName(string name)
        {
            _proc.Name = name;
            return this;
        }

        public ISprocBuilderComplete AndParameters(params Parameter[] parameters)
        {
            _proc.Parameters = new List<Parameter>(parameters);
            return this;
        }

        public IStoredProcedure Build() {
            return _proc;
        }

        public ISprocBuilderParams AndParameter(string name, object value)
        {
            _proc.AddParameter(name, value);
            return this;
        }
    }
}
