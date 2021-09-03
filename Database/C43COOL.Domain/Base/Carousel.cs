using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    public class Carousel : BaseEntity
    {
        /// <summary>
        /// 轮播图名称
        /// </summary>
        public string Title { get; set; }

        public int? Sort { get; set; }

        public string FileId { get; set; }

        public string Link { get; set; }
    }
}
