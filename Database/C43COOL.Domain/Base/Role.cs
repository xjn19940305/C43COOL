using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    public class Role
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public string Description { get; set; }

        public int Sort { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModify { get; set; }

        public ICollection<RoleModules> RoleMenus { get; set; } = new HashSet<RoleModules>();
    }
}
