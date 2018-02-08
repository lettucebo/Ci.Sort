using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ci.Mvc.Sort.Enums;
using Ci.Mvc.Sort.Models;

namespace Ci.Mvc.Sort
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, SortOrder sort)
        {
            if (sort == null)
                sort = new SortOrder();

            if (string.IsNullOrWhiteSpace(sort.Key))
            {
                Type t = typeof(T);
                var propInfos = t.GetProperties();
                sort.Key = propInfos.OrderByDescending(x => x.Name == "Id").First().Name;
            }

            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, sort.Key);
            var exp = Expression.Lambda(prop, param);
            string method = sort.Order == Order.Up ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { query.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
            return query.Provider.CreateQuery<T>(mce);
        }
    }
}
