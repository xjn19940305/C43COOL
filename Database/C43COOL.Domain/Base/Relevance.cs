using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    /// <summary>
    /// 多对多映射表 比如用户角色 角色模块
    /// </summary>
    public class Relevance
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 存储用户ID 角色ID  主表ID
        /// </summary>
        public string FirstId { get; set; }
        /// <summary>
        /// 文章ID  课程ID 资源ID 等等 从表ID
        /// </summary>
        public string SencondId { get; set; }
        /// <summary>
        /// 标识
        /// </summary>
        public string Key { get; set; }

        public string CascadeId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifyDate { get; set; }
    }
}
