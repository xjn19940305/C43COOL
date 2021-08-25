using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.Department.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentCreateModel, Department>().ReverseMap();

            CreateMap<DepartmentModifyModel, Department>().ReverseMap();

            CreateMap<DepartmentInfoModel, Department>().ReverseMap();
        }
    }
}
