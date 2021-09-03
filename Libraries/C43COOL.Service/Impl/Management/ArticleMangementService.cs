using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.Article.Management;
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
    public class ArticleMangementService : IArticleMangementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;

        public ArticleMangementService(
             C43DbContext dbContext,
             IMapper mapper
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task Create(ArticleCreateModel dto)
        {
            var entity = mapper.Map<Article>(dto);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        public async Task Update(ArticleModifyModel dto)
        {
            var entity = await dbContext.Article.FirstOrDefaultAsync(x => x.Id == dto.Id);
            var data = mapper.Map(dto, entity);
            data.DateModify = DateTime.UtcNow;
            dbContext.Update(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(string[] ids)
        {
            var list = new List<Article>();
            var data = await dbContext.Article.Where(x => ids.Contains(x.Id)).ToListAsync();
            dbContext.RemoveRange(data);
            await dbContext.SaveChangesAsync();
        }

        public async Task<ArticleDTO> Get(string Id)
        {
            var data = mapper.Map<ArticleDTO>(await dbContext.Article.FirstOrDefaultAsync(x => x.Id == Id));
            return data;
        }

        public async Task<PagedViewModel> Query(ArticleRequest request)
        {
            var list = dbContext.Article
                 .OrderByDescending(x => x.DateCreated)
                 .AsNoTracking();
            var title = request.Title?.Trim();
            var Author = request.Author?.Trim();
            var CategoryName = request.Name?.Trim();
            if (!string.IsNullOrWhiteSpace(title))
                list = list.Where(x => x.Title.Contains(title));
            if (!string.IsNullOrWhiteSpace(Author))
                list = list.Where(x => x.Author.Contains(Author));
            if (!string.IsNullOrWhiteSpace(request.Id))
                list = list.Where(x => x.Id == request.Id);
            if (!string.IsNullOrWhiteSpace(CategoryName))
                list = list.Where(x => x.Category.Name.Contains(CategoryName));
            var model = new PagedViewModel
            {
                TotalElements = list.Count()
            };
            var dt = await mapper.ProjectTo<ArticleDTO>(list
                    .Skip((request.page - 1) * request.pageSize).Take(request.pageSize)
                    .Include(x => x.Category))
                    .ToListAsync();
            model.Data = dt;
            return model;
        }

    }
}
