using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public class InMemoryStoredProcedureBuilder :
        ISprocBuilder, 
        ISprocBuilderParams, 
        ISprocBuilderComplete
    {
        private string _name;
        private List<Parameter> _parameters;
        private InMemoryStoredProcMapping _mappings;

        public InMemoryStoredProcedureBuilder(IDbConnection connection, InMemoryStoredProcMapping mapping)
        {
            _mappings = mapping;
            _parameters = new List<Parameter>();
        }

        public ISprocBuilderParams WithName(string name)
        {
            _parameters.Clear();
            _name = name;
            return this;
        }

        public ISprocBuilderComplete AndParameters(params Parameter[] parameters)
        {
            _parameters.AddRange(parameters);
            return this;
        }

        public IStoredProcedure Build() {
            Tuple<Func<List<Parameter>, DbDataReader>, Action<List<Parameter>>> func = _mappings.GetProc(_name);

            if (func == null) {
                throw new ArgumentException("No procedure with name " + _name + " has been defined");
            }

            return new InMemoryStoredProcedure(func.Item1, func.Item2) { Parameters = _parameters };
        }

        public ISprocBuilderParams AndParameter(string name, object value)
        {
            _parameters.Add(new Parameter(name, value));
            return this;
        }

        public void UseTransaction(System.Data.SqlClient.SqlTransaction transaction)
        {
            //do nothing
        }
    }
}
