using Ci.Sort.Enums;
using Ci.Sort.Models;
using System.Collections.Generic;
using System.ComponentModel;
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
            PropertyDescriptor baseProp = null;

            if (keyProp == null)
            {
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
                    query = sort.Order == Order.Ascending
                        ? query.OrderBy(x => keyProp.GetValue(baseProp.GetValue(x)))
                        : query.OrderByDescending(x => keyProp.GetValue(baseProp.GetValue(x)));
                }
                else
                {
                    //todo throw exception????
                }
            }
            else
            {
                query = sort.Order == Order.Ascending
                    ? query.OrderBy(x => keyProp.GetValue(x))
                    : query.OrderByDescending(x => keyProp.GetValue(x));

            }
            return query;
        }
    }
}
