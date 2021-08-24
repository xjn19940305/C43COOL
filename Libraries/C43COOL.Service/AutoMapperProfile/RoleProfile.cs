using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.Role.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleInfoModel, Role>().ReverseMap();

            CreateMap<RoleCreateModel, Role>().ReverseMap();

            CreateMap<RoleModifyModel, Role>().ReverseMap();
        }
    }
}
