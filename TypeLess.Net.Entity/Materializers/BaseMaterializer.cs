using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity.Materializers
{
    public abstract class BaseMaterializer<T> : IMaterializer<T> where T : class
    {
        public int ExpectedSize { get; set; }
        private IDataReader _reader;
        private Dictionary<string, int> _ordinals = new Dictionary<string, int>(200);

        public BaseMaterializer(int expectedSize)
        {
            this.ExpectedSize = expectedSize;
        }

        public IObjectSet<T> ToObjectSet(System.Data.IDataReader reader) {
            var rep = new InMemoryObjectSet<T>();
            var res = ToList(reader);
            foreach (var item in res)
            {
                rep.AddObject(item);
            }
            
            return rep;
        }

        public IList<T> ToList(System.Data.IDataReader reader)
        {
            _reader = reader;
            try
            {
                List<T> res = new List<T>(ExpectedSize);

                while (_reader.Read())
                {
                    var ent = ToEntity(_reader);
                    if (ent != null)
                    {
                        res.Add(ent);
                    }
                }
                return res;
            }
            finally
            {

                NextResultSet(_reader);
            }

        }

        public byte[] GetBytes(string column)
        {
            int columnIndex = _reader.GetOrdinal(column);
            int length = (int)_reader.GetBytes(columnIndex, 0, null, 0, 0);
            byte[] buffer = new byte[length];
            int index = 0;

            while (index < length)
            {
                int bytesRead = (int)_reader.GetBytes(columnIndex, index,
                                                buffer, index, length - index);
                index += bytesRead;
            }

            return buffer;
        }

        public T FirstOrDefault(System.Data.IDataReader reader)
        {
            _reader = reader;
            try
            {
                if (_reader.Read())
                {
                    return ToEntity(_reader);
                }
                else
                {
                    return null;
                }
            }
            finally
            {
                NextResultSet(_reader);
            }
            
            
        }

        public BaseMaterializer<T> NextResultSet(System.Data.IDataReader reader)
        {
            _ordinals.Clear();
            reader.NextResult();
            return this;
        }

        //protected T1 Get<T1>(string column)
        //{
        //    return Materializer.Get<T1>(_reader, column);
        //}

        protected bool? IntToBool(string column)
        {
            return Materializer.IntToBool(_reader, column);
        }

        protected TimeSpan? LongToTimespan(string column) {
            return Materializer.LongToTimespan(_reader, column);
        }

        protected abstract T ToEntity(System.Data.IDataReader reader);

        
        protected T1 Get<T1>(string column)
        {
            object val;
            if (_ordinals.ContainsKey(column))
            {
                int ordinal = _ordinals[column];
                val = _reader[ordinal];
            }
            else {
                int ordinal = _reader.GetOrdinal(column);
                _ordinals.Add(column, ordinal);
                val = _reader[ordinal];
            }
            
            if (val is DBNull)
            {
                return default(T1);
            }
            return (T1)val;
        }

        //protected bool? IntToBool(string column)
        //{
        //    short val;
        //    if (_ordinals.ContainsKey(column))
        //    {
        //        int ordinal = _ordinals[column];
        //        if (_reader.IsDBNull(ordinal)) {
        //            return null;
        //        }
        //        val = _reader.GetInt16(ordinal);
        //    }
        //    else
        //    {
        //        int ordinal = _reader.GetOrdinal(column);
        //        if (_reader.IsDBNull(ordinal))
        //        {
        //            return null;
        //        }
        //        _ordinals.Add(column, ordinal);
        //        val = _reader.GetInt16(ordinal);
        //    }

        //    return val == 1 ? true : false;
        //}
         
         
    }
}
