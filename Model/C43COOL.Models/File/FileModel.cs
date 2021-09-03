using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.File
{
    /// <summary>
    /// 文件元数据
    /// </summary>
    public class FileModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 存储位置
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 文件类别
        /// </summary>
        public string Type { get; set; }
    }
}
