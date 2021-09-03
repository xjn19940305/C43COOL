using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Domain.Base
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int? Sort { get; set; }
        public ICollection<Article> Articles { get; set; } = new HashSet<Article>();
    }
}
