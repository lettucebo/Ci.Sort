using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ci.Sort.Enums;
using Ci.Sort.Models;

namespace Ci.Sort
{
    public class Sort
    {
        public static IEnumerable<T> SortEnumerable<T>(IEnumerable<T> query, SortOrder sort)
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
                return sort.Order == Order.Ascending
                   ? query.OrderBy(x => keyProp.GetValue(x))
                   : query.OrderByDescending(x => keyProp.GetValue(x));

            return SortByInternalProperties(query, sort, properties);
        }

        public static IEnumerable<T> SortByInternalProperties<T>(IEnumerable<T> query, SortOrder sort,
            PropertyDescriptorCollection properties)
        {
            if (properties == null || properties.Count == 0)
            {
                throw new ArgumentException("Warning - Sort Property wasn't founded!", nameof(properties));
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
                query = sort.Order == Order.Ascending
                    ? query.OrderBy(GetValue)//todo if null - here will be an exception
                    : query.OrderByDescending(GetValue);//todo if null - here will be an exception
            }
            else
            {
                throw new ArgumentException("Sort Property wasn't founded!", nameof(keyProp));
            }

            return query;
        }

        public static IQueryable<T> SortQueryable<T>(IQueryable<T> query, SortOrder sort)
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

            return SortQueryableByInternalProperties(query, sort, properties);
        }

        public static IQueryable<T> SortQueryableByInternalProperties<T>(IQueryable<T> query, SortOrder sort,
            PropertyDescriptorCollection properties)
        {
            if (properties == null || properties.Count == 0)
            {
                throw new ArgumentException("Warning - Sort Property wasn't founded!", nameof(properties));
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

            throw new ArgumentException("Sort Property wasn't founded!", nameof(keyProp));
        }
    }
}
