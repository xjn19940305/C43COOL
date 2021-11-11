using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.Carousel.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class CarouselProfile : Profile
    {
        public CarouselProfile()
        {
            CreateMap<CarouselCreateModel, Carousel>().ReverseMap();
            CreateMap<CarouselModifyModel, Carousel>().ReverseMap();
            CreateMap<CarouselMangementDto, Carousel>().ReverseMap();
        }
    }
}
