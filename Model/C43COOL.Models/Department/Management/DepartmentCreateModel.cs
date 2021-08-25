using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Department.Management
{
    public class DepartmentCreateModel
    {
        public string Name { get; set; }

        public int Sort { get; set; }

        public string ParentId { get; set; }
    }

    public class DepartmentModifyModel : DepartmentCreateModel
    {
        public string Id { get; set; }

        public string CascadeId { get; set; }
    }
}
