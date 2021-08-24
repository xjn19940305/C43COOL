using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Paging
{
    public class PagedViewModel
    {
        public dynamic Data { get; set; }

        public int TotalElements { get; set; }
    }
}
