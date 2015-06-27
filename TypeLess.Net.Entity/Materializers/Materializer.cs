using System;

namespace TypeLess.Net.Entity.Materializers
{
    public static class Materializer
    {
        public static T1 Get<T1>(System.Data.IDataReader reader, string column)
        {
            var val = reader[column];
            if (val is DBNull)
            {
                return default(T1);
            }
            return (T1)val;
        }

        public static T1 Get<T1>(System.Data.IDataReader reader, string column, T1 defaultValue)
        {
            var val = reader[column];
            if (val is DBNull)
            {
                return defaultValue;
            }
            return (T1)val;
        }

        public static bool? IntToBool(System.Data.IDataReader reader, string column)
        {
            var val = reader[column];
            if (val is DBNull)
            {
                return null;
            }

            int res = (short)val;

            return res == 1 ? true : false;
        }

        public static TimeSpan? LongToTimespan(System.Data.IDataReader reader, string column)
        {
            var val = reader[column];
            if (val is DBNull)
            {
                return null;
            }

            long res = (long)val;

            return new TimeSpan(res);
        }
    }
}
