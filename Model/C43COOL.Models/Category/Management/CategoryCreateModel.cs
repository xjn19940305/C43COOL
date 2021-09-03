using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Category.Management
{
    public class CategoryCreateModel
    {
        public string CategoryName { get; set; }

        public int ParentId { get; set; }
        public int? Sort { get; set; }
    }

    public class CategoryModifyModel : CategoryCreateModel
    {
        public string Id { get; set; }
    }

    public class CategoryDTO
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }

        public int ParentId { get; set; }
        public int? Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyDate { get; set; }
    }
}
