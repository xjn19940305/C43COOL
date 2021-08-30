using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.Modules.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class ModulesProfile : Profile
    {
        public ModulesProfile()
        {
            CreateMap<ModulesCreateModel, Modules>().ReverseMap();

            CreateMap<ModulesModifyModel, Modules>().ReverseMap();

            CreateMap<ModulesInfoModel, Modules>().ReverseMap();

            CreateMap<ModuleViewModel, Modules>().ReverseMap();
        }
    }
}
