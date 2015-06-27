using System;
using System.Linq;
using System.Text;
using TypeLess.Net.Entity.Other;

namespace TypeLess.Net.Entity
{
    public static class QueryExtensions
    {
        public static IQueryable<T> GetPage<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return query.Skip((page - 1) * pageSize).Take(pageSize);

        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string memberName, bool sortAsc)
        {
            var parameters = memberName.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder(100);
            foreach (var item in parameters)
            {
                if (item.ToLower().Contains("asc") || item.ToLower().Contains("desc"))
                {
                    sb.Append(item).Append(",");
                }
                else
                {
                    sb.Append(item).Append((sortAsc ? " asc" : " desc")).Append(",");
                }
            }
            sb = sb.Remove(sb.Length - 1, 1);

            return query.OrderByDyn<T>(sb.ToString());
        }
    }
}
