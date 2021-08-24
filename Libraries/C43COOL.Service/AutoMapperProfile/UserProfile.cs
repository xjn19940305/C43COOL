using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.User.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfoModel, User>().ReverseMap();

            CreateMap<UserCreateModel, User>().ReverseMap();

            CreateMap<UserModifyModel, User>().ReverseMap();
        }
    }
}
