using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Department.Management
{
    public class DepartmentInfoModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }

        public string ParentId { get; set; }

        public string CascadeId { get; set; }
    }
}
