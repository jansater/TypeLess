using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using TypeLess.Net.Entity.Contracts;


namespace TypeLess.Net.Entity
{
    public class InMemoryStoredProcedure : IStoredProcedure
    {
        private Action<List<Parameter>> _updateAction;
        private Func<List<Parameter>, DbDataReader> _createReaderFunc;

        internal InMemoryStoredProcedure(Func<List<Parameter>, DbDataReader> createReaderFunc, Action<List<Parameter>> updateAction = null)
        {
            _createReaderFunc = createReaderFunc;
            _updateAction = updateAction;
            Parameters = new List<Parameter>();
        }

        internal string Name { get; set; }

        internal List<Parameter> Parameters { get; set; }

        internal IDbConnection Connection { get; set; }

        public InMemoryStoredProcedure(IDbConnection connection, string procedureName, params Parameter[] parameters)
        {
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
            //this function will get called by the clients...they assume that they will get a datareader with all the data
            return responseCallback(_createReaderFunc(Parameters));
        }

        public void ExecuteUpdate()
        {
            _updateAction(Parameters);
        }

    }
}
