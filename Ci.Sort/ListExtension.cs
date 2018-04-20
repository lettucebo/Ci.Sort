using Ci.Sort.Enums;
using Ci.Sort.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Ci.Sort
{
    public static class ListExtension
    {
        public static List<T> Sort<T>(this List<T> query, SortOrder sort)
        {
            var queryEnumerable = query.AsEnumerable();
            return Ci.Sort.Sort.SortEnumerable(queryEnumerable, sort).ToList();
        }
    }
}
