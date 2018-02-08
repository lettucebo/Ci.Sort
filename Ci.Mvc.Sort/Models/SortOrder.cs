using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ci.Mvc.Sort.Enums;

namespace Ci.Mvc.Sort.Models
{
    public class SortOrder
    {
        public string Key { get; set; }

        public Order Order { get; set; }
    }
}
