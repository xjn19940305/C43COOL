using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Infrastructure;
using C43COOL.Models.Modules.Management;
using C43COOL.Service.Global;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Permission.Strategy
{
    public class NormalAuthStrategy : IAuthStrategy
    {
        private readonly IUserContext userContext;
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;

        public NormalAuthStrategy(
            IUserContext userContext,
            C43DbContext dbContext,
            IMapper mapper
            )
        {
            this.userContext = userContext;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public List<ModuleViewModel> Modules
        {
            get
            {
                var ModuleIds = dbContext.Relevances.Where(x => x.Key == Define.ROLEMODULE && Role.Select(g => g.Id).Contains(x.FirstId))
                    .Select(f => f.SencondId)
                    .ToList();
                return mapper.ProjectTo<ModuleViewModel>(dbContext.Modules.Where(f => ModuleIds.Contains(f.Id)).AsNoTracking()).ToList();
            }
        }
        public List<Role> Role
        {
            get
            {
                var RoleIds = dbContext.Relevances.Where(x => x.Key == Define.USERROLE && x.FirstId == userContext.Id)
                    .Select(f => f.SencondId)
                    .ToList();
                return dbContext.Roles.Where(f => RoleIds.Contains(f.Id)).ToList();
            }
        }
    }
}
