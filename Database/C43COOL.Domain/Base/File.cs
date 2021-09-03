using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    public class Files : BaseEntity
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
    }
}
