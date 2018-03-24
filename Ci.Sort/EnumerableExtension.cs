using Ci.Sort.Enums;
using Ci.Sort.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Ci.Sort
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> query, SortOrder sort)
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

        internal static IEnumerable<T> SortByInternalProperties<T>(IEnumerable<T> query, SortOrder sort,
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
                query = sort.Order == Order.Ascending
                    ? query.OrderBy(GetValue)//todo if null - here will be an exception
                    : query.OrderByDescending(GetValue);//todo if null - here will be an exception
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
