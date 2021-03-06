using AutoMapper;
using C43COOL.Domain.Base;
using C43COOL.Models.JobModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.AutoMapperProfile
{
    public class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<JobScheduleDTO, JobSchedule>().ReverseMap();
        }
    }
}
