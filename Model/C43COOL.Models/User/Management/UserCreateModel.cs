using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.User.Management
{
    public class UserCreateModel
    {
        public string Name { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UserModifyModel : UserCreateModel
    {
        public string Id { get; set; }
    }
}
