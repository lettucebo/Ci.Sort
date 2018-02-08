using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ci.Mvc.Sort.Enums;
using Ci.Mvc.Sort.Models;

namespace Ci.Mvc.Sort
{
    public static class ListExtension
    {
        public static List<T> Sort<T>(this List<T> query, SortOrder sort)
        {
            if (sort == null)
                sort = new SortOrder();

            if (string.IsNullOrWhiteSpace(sort.Key))
            {
                Type t = typeof(T);
                var propInfos = t.GetProperties();
                sort.Key = propInfos.OrderByDescending(x => x.Name == "Id").First().Name;
            }

            PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T)).Find(sort.Key, true);

            query = sort.Order == Order.Up ? query.OrderBy(x => prop.GetValue(x)).ToList() : query.OrderByDescending(x => prop.GetValue(x)).ToList();
            return query;
        }
    }
}
