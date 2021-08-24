using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.Modules.Management;
using C43COOL.Models.Paging;
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
    public class ModulesManagementService : IModulesManagementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserContext userContext;
        public ModulesManagementService(
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
        public async Task<PagedViewModel> Query(ModulesQueryModel request)
        {
            var data = await dbContext.Modules.OrderBy(x => x.Sort).ToListAsync();
            return new PagedViewModel
            {
                Data = data,
                TotalElements = data.Count
            };
        }
        public async Task Create(ModulesCreateModel model)
        {
            var entity = mapper.Map<Modules>(model);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        public async Task Modify(ModulesModifyModel model)
        {
            var data = await dbContext.Modules.FirstOrDefaultAsync(x => x.Id == model.Id);
            var menu = mapper.Map(model, data);
            menu.DateModify = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(string[] ids)
        {
            var list = await dbContext.Modules.Where(x => ids.Contains(x.Id)).ToListAsync();
            dbContext.RemoveRange(list);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ModulesInfoModel> Get(string Id)
        {
            return mapper.Map<ModulesInfoModel>(await dbContext.Modules.FirstOrDefaultAsync(x => x.Id == Id));
        }




    }
}
