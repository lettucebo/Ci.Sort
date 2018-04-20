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
            return Ci.Sort.Sort.SortQueryable(query, sort);
        }
    }
}
