using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.Modules.Management;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Permission.Strategy
{
    public class AdminAuthStrategy : IAuthStrategy
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;

        public AdminAuthStrategy(
            C43DbContext dbContext,
            IMapper mapper
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public List<ModuleViewModel> Modules
        {
            get
            {
                return mapper.ProjectTo<ModuleViewModel>(dbContext.Modules.AsNoTracking()).ToList();
            }
        }

        public List<Role> Role
        {
            get
            {
                return dbContext.Roles.AsNoTracking().ToList();
            }
        }
    }
}
