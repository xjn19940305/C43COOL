using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    public class RoleModules
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RoleId { get; set; }

        public string ModulesId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? DateModify { get; set; }

        public Role Role { get; set; }

        public Modules Modules { get; set; }
    }
}
