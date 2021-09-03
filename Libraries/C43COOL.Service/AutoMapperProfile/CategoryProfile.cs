using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.Category.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateModel, Category>().ReverseMap();
            CreateMap<CategoryModifyModel, Category>().ReverseMap();
            CreateMap<CategoryDTO, Category>().ReverseMap();
        }
    }
}
