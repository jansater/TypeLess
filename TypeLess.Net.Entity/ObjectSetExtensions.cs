using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using TypeLess.Net.Entity.Contracts;

namespace TypeLess.Net.Entity
{
    public static class ObjectSetExtensions
    {
        public static bool Add<T>(this IObjectSet<T> set, T obj) where T : class
        {
            if (obj != null && !set.Contains(obj))
            {
                set.AddObject(obj);
                return true;
            }
            return false;
        }

        public static void AddRange<T>(this IObjectSet<T> set, IEnumerable<T> objs, Action<T> objAction = null) where T : class
        {
            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    if (!set.Contains(obj)) {
                        set.AddObject(obj);
                        if (objAction != null)
                        {
                            objAction(obj);

                        }
                    }
                    
                }
            }
        }

        public static IQueryable<TSource> Include<TSource>
          (this IQueryable<TSource> source, string path) where TSource : class
        {
            if (source is InMemoryObjectSet<TSource>)
            {
                return source;
            }

            var objectQuery = source as ObjectQuery<TSource>;
            if (objectQuery != null)
            {
                return objectQuery.Include(path);
            }
            return source;
        }

        public static IQueryable<TSource> Include<TSource>
          (this IQueryable<TSource> source, string[] paths) where TSource : class
        {
            if (source is InMemoryObjectSet<TSource>)
            {
                return source;
            }

            var objectQuery = source as ObjectQuery<TSource>;
            if (objectQuery != null)
            {
                foreach (var path in paths)
                {
                    objectQuery = objectQuery.Include(path);
                }

                return objectQuery;
            }
            return source;
        }

        public static void SetMergeOption<TSource>(this IObjectSet<TSource> source, MergeOption option) where TSource : class
        {
            if (source is InMemoryObjectSet<TSource>)
            {
                return;
            }

            var set = source as ObjectSet<TSource>;
            set.MergeOption = option;
        }

        public static IQueryable<TSource> Where<TSource>(this IObjectSet<TSource> source, string predicate) where TSource : class
        {
            if (source is InMemoryObjectSet<TSource>)
            {
                return source.Matches();
            }

            var set = source as ObjectSet<TSource>;
            return (IQueryable<TSource>)set.Where(predicate);
        }

        public static IQueryable<T> Matches<T>(this IObjectSet<T> source, QueryOptions options, System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression is required");
            }

            if (options == null)
            {
                throw new ArgumentNullException("Query options is null");
            }
            source.SetMergeOption((MergeOption)((int)options.MergeOption));
            
            IQueryable<T> q = source;
            if (options.Includes.Any())
            {
                ObjectQuery<T> objSet = source as ObjectQuery<T>;
                if (objSet != null) {
                    foreach (var include in options.Includes)
                    {
                        objSet = objSet.Include(include);
                    }
                    q = objSet;
                }
                
            }

            q = q.Where(expression);

            if (options != null)
            {
                if (options.SortBy != null)
                {
                    q = q.SortBy(options.SortBy, options.SortAscending);
                }

                if (options.Page > 0 && options.PageSize > 0)
                {
                    q = q.GetPage(options.Page, options.PageSize);
                }
            }

            return q;
        }

        //private void validateInput(params ICriteria<T>[] criteria)
        //{
        //    if (!_validator.IsValid(criteria))
        //    {
        //        throw new ArgumentException("Criteria is not valid: " + _validator.GetValidationErrorsAsString());
        //    }
        //}

        public static IQueryable<T> Matches<T>(this IObjectSet<T> source, QueryOptions options, params ICriteria<T>[] criteria) where T : class
        {
            if (options == null)
            {
                throw new ArgumentNullException("Query options is null");
            }

            //validateInput(criteria);
            source.SetMergeOption((MergeOption)((int)options.MergeOption));
           
            IQueryable<T> query = source;
            if (options.Includes.Any())
            {
                ObjectQuery<T> objSet = source as ObjectQuery<T>;
                if (objSet != null) {
                    foreach (var include in options.Includes)
                    {
                        objSet = objSet.Include(include);
                    }
                    query = objSet;
                }
                
            }

            foreach (var c in criteria)
            {
                query = c.GetMatches(query);
            }

            if (options != null)
            {
                if (options.SortBy != null)
                {
                    query = query.SortBy(options.SortBy, options.SortAscending);
                }

                if (options.Page > 0 && options.PageSize > 0)
                {
                    query = query.GetPage(options.Page, options.PageSize);
                }
            }

            return query;
        }

        public static IQueryable<T> Matches<T>(this IObjectSet<T> source, params ICriteria<T>[] criteria) where T : class
        {
            return Matches(source, new QueryOptions(), criteria);
        }

        public static IQueryable<T> Matches<T>(this IObjectSet<T> source, System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class
        {
            return Matches(source, new QueryOptions(), expression);
        }

        public static void AddRange<T>(this IObjectSet<T> source, IEnumerable<T> list) where T : class {
            foreach (var item in list)
            {
                source.AddObject(item);
            }
        }
    }
}
