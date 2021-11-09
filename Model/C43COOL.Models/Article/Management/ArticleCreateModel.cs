using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Article.Management
{
    public class ArticleCreateModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        public int LinkCount { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumImg { get; set; }

        public string CategoryId { get; set; }
    }

    public class ArticleModifyModel : ArticleCreateModel
    {
        public string Id { get; set; }
    }

    public class ArticleDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// 点击次数
        /// </summary>
        public int LinkCount { get; set; }
        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ThumImg { get; set; }

        public string CategoryId { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
