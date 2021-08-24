using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Paging
{
    public class PagingModel
    {
        public int pageSize { get; set; } = 10;
        public int page { get; set; } = 1;
    }
}
