using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Models.Carousel.Management
{
    public class CarouselMangementDto
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public int? Sort { get; set; }

        public string FileId { get; set; }

        public string Link { get; set; }
    }
}
