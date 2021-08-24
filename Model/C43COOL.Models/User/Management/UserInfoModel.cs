using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.User.Management
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }

        public string Password { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int Gender { get; set; }
        public string Locale { get; set; }
        public string ZoneInfo { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string OpenId { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string UnionId { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// 账号启用禁用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
