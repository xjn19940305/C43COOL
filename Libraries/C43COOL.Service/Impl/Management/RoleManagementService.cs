using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Infrastructure;
using C43COOL.Models.Paging;
using C43COOL.Models.Role.Management;
using C43COOL.Service.Global;
using C43COOL.Service.Interface.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Impl.Management
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserContext userContext;
        public RoleManagementService(
            C43DbContext dbContext,
            IMapper mapper,
            IConfiguration configuration,
            IUserContext userContext
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.configuration = configuration;
            this.userContext = userContext;
        }

        public async Task<PagedViewModel> Query(RoleQueryRequest request)
        {
            var model = new PagedViewModel();
            var data = dbContext.Roles.AsNoTracking();
            model.TotalElements = data.Count();
            model.Data = await data.OrderBy(x => x.Sort)
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize).ToListAsync();
            return model;
        }

        public async Task Create(RoleCreateModel model)
        {
            if (await dbContext.Roles.AnyAsync(x => x.Name == model.Name))
                throw new Exception("角色名称重复!");
            var entity = mapper.Map<Role>(model);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        public async Task Modify(RoleModifyModel model)
        {
            var entity = await dbContext.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);
            var role = mapper.Map(model, entity);
            role.DateModify = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(string[] ids)
        {
            var role = await dbContext.Roles.Where(x => ids.Contains(x.Id)).ToListAsync();
            dbContext.RemoveRange(role);
            await dbContext.SaveChangesAsync();
        }
        public async Task AuthorizeRole(RoleMenuAuthorizeModel model)
        {
            var RoleModule = await dbContext.Relevances.Where(x => x.Key == Define.ROLEMODULE && x.FirstId == model.RoleId).ToListAsync();
            dbContext.RemoveRange(RoleModule);
            foreach (var item in model.ModuleIds)
            {
                await dbContext.AddAsync(new Relevance
                {
                    FirstId = model.RoleId,
                    SencondId = item,
                    Key = Define.ROLEMODULE
                });
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<RoleInfoModel> Get(string Id)
        {
            return mapper.Map<RoleInfoModel>(await dbContext.Roles.FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<string[]> GetRoleModules(string RoleId)
        {
            return await dbContext.Relevances.Where(x => x.Key == Define.ROLEMODULE && x.FirstId == RoleId).Select(x => x.SencondId).ToArrayAsync();
        }

    }
}
