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
            return Ci.Sort.Sort.SortEnumerable(query, sort);
        }
    }
}
