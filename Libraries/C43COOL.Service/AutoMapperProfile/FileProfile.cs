using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<FileModel, Files>()
           .ForMember(p => p.FileType, p => p.MapFrom(src => src.Type))
           .ReverseMap();
        }
    }
}
