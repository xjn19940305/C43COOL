using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    public class UserRole
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string RoleId { get; set; }
        public DateTime DateCreated { get; set; }

        public DateTime? DateModify { get; set; }

        public Role Role { get; set; }

        public User User { get; set; }
    }
}
