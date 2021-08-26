using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.User.Management
{
    public class UserBindModel
    {
        public string UserId { get; set; }

        public string[] RoleIds { get; set; }
    }
}
