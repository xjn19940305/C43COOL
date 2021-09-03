using C43COOL.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Article.Management
{
    public class ArticleRequest : PagingModel
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }
    }
}
