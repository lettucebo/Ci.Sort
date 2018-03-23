using Ci.Sort.Enums;
using Ci.Sort.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Ci.Sort
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, SortOrder sort)
        {
            if (sort == null)
                sort = new SortOrder();

            if (string.IsNullOrWhiteSpace(sort.Key))
            {
                sort.Key = typeof(T)
                    .GetProperties()
                    .OrderByDescending(x => x.Name == "Id")
                    .First()
                    .Name;
            }

            var properties = TypeDescriptor.GetProperties(typeof(T));

            var keyProp = properties
                .Find(sort.Key, true);

            if (keyProp != null)
            {
                var param = Expression.Parameter(typeof(T), "p");
                var prop = Expression.Property(param, keyProp.Name);
                var exp = Expression.Lambda(prop, param);
                var method = sort.Order == Order.Ascending ? "OrderBy" : "OrderByDescending";
                Type[] types = { query.ElementType, exp.Body.Type };
                var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
                return query.Provider.CreateQuery<T>(mce);
            }

            return SortByInternalProperties(query, sort, properties);
        }

        internal static IQueryable<T> SortByInternalProperties<T>(IQueryable<T> query, SortOrder sort,
            PropertyDescriptorCollection properties)
        {
            if (properties == null || properties.Count == 0)
            {
                Debug.WriteLine("Warning - Sort Property wasn't founded!");
                //todo throw an exception????
                return query;
            }

            PropertyDescriptor keyProp = null;
            PropertyDescriptor baseProp = null;
            foreach (PropertyDescriptor property in properties)
            {
                baseProp = property;
                var childProperties = property.GetChildProperties();
                keyProp = childProperties
                    .Find(sort.Key, true);

                if (keyProp != null) break;
            }

            object GetValue(T arg)
            {
                var baseValue = baseProp.GetValue(arg);
                return baseValue != null
                    ? keyProp.GetValue(baseValue)
                    : null;//todo throw an exception????
            }

            if (keyProp != null)
            {
                var param = Expression.Parameter(typeof(T), "p");
                var brop = Expression.Property(param, baseProp.Name);
                var kprop = Expression.Property(brop, keyProp.Name);
                var exp = Expression.Lambda(kprop, param);
                var method = sort.Order == Order.Ascending ? "OrderBy" : "OrderByDescending";
                Type[] types = { query.ElementType, exp.Body.Type };
                var mce = Expression.Call(typeof(Queryable), method, types, query.Expression, exp);
                return query.Provider.CreateQuery<T>(mce);
            }
            else
            {
                Debug.WriteLine("Warning - Sort Property wasn't founded!");
                //todo throw an exception????
            }

            return query;
        }
    }
}
