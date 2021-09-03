using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.File
{
    /// <summary>
    /// 创建文件所需要的参数
    /// </summary>
    public class CreateFileRequest
    {
        public string Name { get; set; }
        public string FileType { get; set; }
        public Stream FileStream { get; set; }
    }
}
