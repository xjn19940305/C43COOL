using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Role.Management
{
    public class RoleCreateModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public int Sort { get; set; }
    }

    public class RoleModifyModel
    {
        public string Id { get; set; }
    }
}
