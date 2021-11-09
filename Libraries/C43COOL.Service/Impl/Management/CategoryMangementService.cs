using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.Category.Management;
using C43COOL.Models.Paging;
using C43COOL.Service.Interface.Management;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Impl.Management
{
    public class CategoryMangementService : ICategoryMangementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;

        public CategoryMangementService(
             C43DbContext dbContext,
             IMapper mapper
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public async Task Create(CategoryCreateModel dto)
        {
            var entity = mapper.Map<Category>(dto);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        public async Task Update(CategoryModifyModel dto)
        {
            var entity = await dbContext.Category.FirstOrDefaultAsync(x => x.Id == dto.Id);
            var data = mapper.Map(dto, entity);
            data.DateModify = DateTime.UtcNow;
            dbContext.Update(data);
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(string[] Id)
        {
            var list = new List<Category>();
            var data = await dbContext.Category.Where(x => Id.Contains(x.Id)).ToListAsync();
            dbContext.RemoveRange(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CategoryDTO> Get(string Id)
        {
            var data = mapper.Map<CategoryDTO>(await dbContext.Category.FirstOrDefaultAsync(x => x.Id == Id));
            return data;
        }

        public async Task<PagedViewModel> Query(CategoryRequest request)
        {
            var data = await mapper.ProjectTo<CategoryDTO>(dbContext.Category
                   .OrderBy(x => x.Sort))
                   .ToListAsync();
            return new PagedViewModel
            {
                Data = data,
                TotalElements = data.Count
            };
        }
    }
}
