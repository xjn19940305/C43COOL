using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Role.Management
{
    public class RoleInfoModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Sort { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateModify { get; set; }
    }
}
