using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public abstract class InMemoryStoredProcMapping
    {
        private Dictionary<string, Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>>> _procMap;

        public InMemoryStoredProcMapping()
        {
            _procMap = new Dictionary<string, Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>>>();
        }

        internal Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>> GetProc(string name)
        {
            if (!_procMap.ContainsKey(name))
            {
                return null;
            }

            return _procMap[name];
        }

        protected void AddGetProc(string name, Func<List<Parameter>, System.Data.Common.DbDataReader> def)
        {

            if (_procMap.ContainsKey(name))
            {
                var current = _procMap[name];
                _procMap[name] = new Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>>(def, current.Item2);
            }
            else
            {
                _procMap.Add(name, new Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>>(def, null));
            }

        }

        protected void AddUpdateProc(string name, Action<List<Parameter>> def)
        {

            if (_procMap.ContainsKey(name))
            {
                var current = _procMap[name];
                _procMap[name] = new Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>>(current.Item1, def);
            }
            else
            {
                _procMap.Add(name, new Tuple<Func<List<Parameter>, System.Data.Common.DbDataReader>, Action<List<Parameter>>>(null, def));
            }

        }

        internal interface IResultSet {
            SortedDictionary<string, object> Properties { get; }
            int RowCount { get; }
            bool NextRecord { get; }
        }

        internal class ResultSet<T> : IResultSet where T : class, new() {

            private int _currentRow;
            private int _nRows;
            private SortedDictionary<string, object> _properties = new SortedDictionary<string,object>();
            private Action<T, dynamic, int> _modAction;
            private Type _resultSetType;
            private Func<IEnumerable<T>> _generator;
            
            public static IResultSet New(int rowsWanted, Action<T, dynamic, int> handler) {
               var rs = new ResultSet<T>();
                rs._currentRow = 0;
                rs._modAction = handler;
                rs._nRows = rowsWanted;
                rs._resultSetType = typeof(T);
                

                rs._generator = () =>
                {
                    return RandomData.CreateList<T>(1);
                };
                

               return rs;
            }

            public static IResultSet New(Func<IEnumerable<T>> generator, Action<T, dynamic, int> handler = null)
            {
                if (generator == null) {
                    throw new ArgumentNullException("generator");
                }

                var rs = new ResultSet<T>();
                rs._currentRow = 0;
                rs._modAction = handler;
                rs._nRows = generator().Count();
                rs._resultSetType = typeof(T);
                rs._generator = generator;

                return rs;
            }

            private ResultSet()
            {

            }

            public SortedDictionary<string, object> Properties { 
                get {
                return _properties;
            }
            }

        
            public int RowCount
            {
                get { return _nRows; }
            }
        

            public bool NextRecord
            {
	            get { 

                    _properties.Clear();

                    if (_currentRow >= _nRows) {
                        return false;   
                    }

                    //lets fill the current record
                    var t = _generator();
                    ExpandoObject newProps = new ExpandoObject();
                    //make sure that the generator is in range...
                    var index = _currentRow % t.Count();
                    T currentItem = t.ElementAt(index);
                    if (_modAction != null)
                    {
                        _modAction(currentItem, newProps, _currentRow);
                    }

                    if (currentItem != null) {
                        _properties = RandomData.CreateDictionaryFromObject(currentItem);
                        newProps.ToList().ForEach(x =>
                        {
                            _properties[x.Key] = x.Value;
                        });
                    }
                    
                    _currentRow++;
                    return true;
                }
            }
}

        internal class FakeReader : DbDataReader
        {


            public FakeReader()
            {
                
            }

            public void AddResultHandler<T>(int rowsWanted, Action<T, dynamic, int> handler) where T : class, new()
            {
                _resultSets.Add(ResultSet<T>.New(rowsWanted, handler));
            }

            public void AddResultHandler<T>(Func<IEnumerable<T>> generator, Action<T, dynamic, int> handler = null) where T : class, new()
            {
                _resultSets.Add(ResultSet<T>.New(generator, handler));
            }

            private List<IResultSet> _resultSets = new List<IResultSet>();
            private int _currentResultSet = 0;

            public override void Close()
            {

            }

            public override int Depth
            {
                get { return 1; }
            }

            public override int FieldCount
            {
                get { return _resultSets[_currentResultSet].Properties.Count; }
            }

            public override bool GetBoolean(int ordinal)
            {
                return (bool)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override byte GetByte(int ordinal)
            {
                return (byte)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
            {
                return 0L;
            }

            public override char GetChar(int ordinal)
            {
                return (char)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
            {
                throw new NotSupportedException();
            }

            public override string GetDataTypeName(int ordinal)
            {
                return (_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).GetType().Name;
            }

            public override DateTime GetDateTime(int ordinal)
            {
                return (DateTime)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override decimal GetDecimal(int ordinal)
            {
                return (decimal)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override double GetDouble(int ordinal)
            {
                return (double)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override System.Collections.IEnumerator GetEnumerator()
            {
                return _resultSets[_currentResultSet].Properties.GetEnumerator();
            }

            public override Type GetFieldType(int ordinal)
            {
                return (_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value.GetType();
            }

            public override float GetFloat(int ordinal)
            {
                return (float)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override Guid GetGuid(int ordinal)
            {
                return (Guid)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override short GetInt16(int ordinal)
            {
                return (short)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override int GetInt32(int ordinal)
            {
                return (int)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override long GetInt64(int ordinal)
            {
                return (long)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override string GetName(int ordinal)
            {
                return (_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Key;
            }

            public override int GetOrdinal(string name)
            {
                return _resultSets[_currentResultSet].Properties.ToArray().Select((x, i) => new { Name = x.Key, Index = i }).Where(x => x.Name == name).Select(x => x.Index).First();
            }

            public override System.Data.DataTable GetSchemaTable()
            {
                throw new NotSupportedException();
            }

            public override string GetString(int ordinal)
            {
                return (string)(_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override object GetValue(int ordinal)
            {
                return (_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value;
            }

            public override int GetValues(object[] values)
            {
                throw new NotSupportedException();
            }

            public override bool HasRows
            {
                get { return _resultSets[_currentResultSet].RowCount > 0; }
            }

            public override bool IsClosed
            {
                get { return false; }
            }

            public override bool IsDBNull(int ordinal)
            {
                return (_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value == null;
            }

            //this can be of another type ... how should we handle this?
            public override bool NextResult()
            {
                if (_currentResultSet + 1 >= _resultSets.Count)
                {
                    return false;
                }
                _currentResultSet++;
                return true;
            }

            public override bool Read()
            {
                return _resultSets[_currentResultSet].NextRecord;
            }

            public override int RecordsAffected
            {
                get { return -1; }
            }

            public override object this[string name]
            {
                get { return _resultSets[_currentResultSet].Properties[name]; }
            }

            public override object this[int ordinal]
            {
                get { return (_resultSets[_currentResultSet].Properties.ToArray()[ordinal]).Value; }
            }
        }

    }
}
