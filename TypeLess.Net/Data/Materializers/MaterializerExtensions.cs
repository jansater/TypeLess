using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess.Net.Contracts;

namespace TypeLess.Net.Data.Materializers
{
    public static class MaterializerExtensions
    {

        public static TranslateType FirstOrDefault<TranslateType>(this ObjectContext targetObject, DbDataReader reader) where TranslateType : class
        {
            TranslateType res = null;
            if (reader.HasRows)
            {
                string setName = GetSetName<TranslateType>(targetObject);
                res = targetObject.Translate<TranslateType>(reader, setName, MergeOption.NoTracking).FirstOrDefault();
            }

            reader.NextResult();
            return res;
        }

        public static TranslateType FirstOrDefault<TranslateType>(this DbDataReader reader, IMaterializer<TranslateType> materializer) where TranslateType : class
        {
            TranslateType res = null;
            if (reader.HasRows)
            {
                //string setName = GetSetName<TranslateType>(targetObject);
                res = materializer.FirstOrDefault(reader);
            }

            reader.NextResult();
            return res;
        }

        public static TranslateType ReadScalar<TranslateType>(this ObjectContext targetObject, int ordinal, bool proceedToNextResultSet, DbDataReader reader)
        {
            if (!reader.HasRows || reader.IsClosed)
            {

                if (proceedToNextResultSet)
                {
                    reader.NextResult();
                }

                return default(TranslateType);
            }

            if (!reader.Read())
            {

                if (proceedToNextResultSet)
                {
                    reader.NextResult();
                }

                return default(TranslateType);
            }

            var res = (TranslateType)reader.GetValue(ordinal);

            if (proceedToNextResultSet)
            {
                reader.NextResult();
            }

            return res;
        }

        private static string GetSetName<T>(ObjectContext targetObject)
        {
            string className= typeof(T).Name;
            var container = targetObject.MetadataWorkspace.GetEntityContainer(targetObject.DefaultContainerName, DataSpace.CSpace);
            string setName = (from meta in container.BaseEntitySets
                              where meta.ElementType.Name == className
                              select meta.Name).First();
            return setName;
        }

        public static IEnumerable<TranslateType> ToList<TranslateType>(this ObjectContext targetObject, DbDataReader reader)
        {
            IEnumerable<TranslateType> res = null;
            if (reader.HasRows)
            {
                string setName = GetSetName<TranslateType>(targetObject);
                res = targetObject.Translate<TranslateType>(reader, setName, MergeOption.NoTracking).ToList();
            }
            reader.NextResult();
            return res.EmptyIfNull();
        }

        public static IEnumerable<TranslateType> ToList<TranslateType>(this DbDataReader reader, IMaterializer<TranslateType> materializer)
        {
            IEnumerable<TranslateType> res = null;
            if (reader.HasRows)
            {
                res = materializer.ToList(reader);
            }
            reader.NextResult();
            return res.EmptyIfNull();
        }

        public static IEnumerable<TranslateType> ToList<TranslateType>(this DbDataReader reader, int expectedCount, Func<TranslateType> func)
        {
            List<TranslateType> res = new List<TranslateType>(expectedCount);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var item = func();
                    if (item != null)
                    {
                        res.Add(item);
                    }

                }
            }
            reader.NextResult();
            return res.EmptyIfNull();
        }

        public static IEnumerable<TranslateType> Join<BaseType, TranslateType>(this ObjectContext targetObject, DbDataReader reader, IEnumerable<BaseType
> parentList, Action<BaseType, IEnumerable<TranslateType>> joinAction)
        {
            var res = ToList<TranslateType>(targetObject, reader);
            if (parentList != null)
            {
                foreach (var item in parentList)
                {
                    joinAction.Invoke(item, res);
                }
            }

            return res;
        }

        public static IEnumerable<TranslateType> Join<BaseType, TranslateType>(this DbDataReader reader, IEnumerable<BaseType
> parentList, Action<BaseType, IEnumerable<TranslateType>> joinAction, IMaterializer<TranslateType> materializer)
        {
            var res = materializer.ToList(reader);
            if (parentList != null)
            {
                foreach (var item in parentList)
                {
                    joinAction.Invoke(item, res);
                }
            }

            return res;
        }

        /// <summary>
        /// Returns an empty collection if the target object is null otherwise the target object itself is returned
        /// </summary>
        private static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> targetObject)
        {
            if (targetObject == null)
            {
                return new T[0];
            }

            return targetObject;
        }

    }
}
