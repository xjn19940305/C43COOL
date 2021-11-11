using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Models.Carousel.Management;
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
    public class CarouselManagementService : ICarouselManagementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IUserContext userContext;
        public CarouselManagementService(
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
        public async Task<PagedViewModel> Query(CarouselQueryModel request)
        {
            var model = new PagedViewModel();
            var data = dbContext.Carousel.AsNoTracking();
            model.TotalElements = data.Count();
            model.Data = await data.OrderBy(x => x.Sort)
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize).ToListAsync();
            return model;
        }
        public async Task Create(CarouselCreateModel model)
        {
            var Carousel = mapper.Map<Carousel>(model);
            await dbContext.AddAsync(Carousel);
            await dbContext.SaveChangesAsync();
        }
        public async Task Modify(CarouselModifyModel model)
        {
            var entity = await dbContext.Carousel.FirstOrDefaultAsync(x => x.Id == model.Id);
            var Carousel = mapper.Map(model, entity);
            Carousel.DateModify = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }

        public async Task Delete(string[] ids)
        {
            var carousels = await dbContext.Carousel.Where(x => ids.Contains(x.Id)).ToListAsync();
            dbContext.RemoveRange(carousels);
            await dbContext.SaveChangesAsync();
        }

        public async Task<CarouselMangementDto> Get(string Id)
        {
            var data = await dbContext.Carousel.FirstOrDefaultAsync(x => x.Id == Id);
            return mapper.Map<CarouselMangementDto>(data);
        }

       

      
    }
}
