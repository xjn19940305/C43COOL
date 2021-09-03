using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Carousel.Management
{
    public class CarouselCreateModel
    {
        public string Title { get; set; }

        public int? Sort { get; set; }

        public string FileId { get; set; }

        public string Link { get; set; }
    }

    public class CarouselModifyModel : CarouselCreateModel
    {
        public string Id { get; set; }
    }
}
