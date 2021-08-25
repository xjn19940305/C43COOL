using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.Department.Management;
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
    public class DepartmentManagementService : IDepartmentManagementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserContext userContext;
        public DepartmentManagementService(
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
        public async Task<PagedViewModel> Query(DepartmentQueryModel request)
        {
            var data = await dbContext.Department.OrderBy(x => x.Sort).ToListAsync();
            return new PagedViewModel
            {
                Data = data,
                TotalElements = data.Count
            };
        }
        public async Task Create(DepartmentCreateModel model)
        {
            var entity = mapper.Map<Department>(model);
            await CaculateCascade(entity);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        public async Task Modify(DepartmentModifyModel model)
        {
            var data = await dbContext.Modules.FirstOrDefaultAsync(x => x.Id == model.Id);
            var menu = mapper.Map(model, data);
            menu.DateModify = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(string[] ids)
        {
            var list = await dbContext.Department.Where(x => ids.Contains(x.Id)).ToListAsync();
            dbContext.RemoveRange(list);
            await dbContext.SaveChangesAsync();
        }

        public async Task<DepartmentInfoModel> Get(string Id)
        {
            return mapper.Map<DepartmentInfoModel>(await dbContext.Department.FirstOrDefaultAsync(x => x.Id == Id));
        }

        private async Task CaculateCascade(Department entity)
        {
            if (string.IsNullOrWhiteSpace(entity.ParentId)) entity.ParentId = null;
            string cascadeId;
            int currentCascadeId = 1; //当前结点的级联节点最后一位
            var sameLevels = await dbContext.Department.Where(o => o.ParentId == entity.ParentId && o.Id != entity.Id).ToListAsync();
            foreach (var obj in sameLevels)
            {
                int objCascadeId = int.Parse(obj.CascadeId.TrimEnd('.').Split('.').Last());
                if (currentCascadeId <= objCascadeId) currentCascadeId = objCascadeId + 1;
            }

            if (!string.IsNullOrEmpty(entity.ParentId))
            {
                var parentOrg = await dbContext.Department.FirstOrDefaultAsync(o => o.Id == entity.ParentId);
                if (parentOrg != null)
                {
                    cascadeId = parentOrg.CascadeId + currentCascadeId + ".";
                }
                else
                {
                    throw new Exception("未能找到该组织的父节点信息");
                }
            }
            else
            {
                cascadeId = ".0." + currentCascadeId + ".";
            }
            entity.CascadeId = cascadeId;
        }
    }
}
