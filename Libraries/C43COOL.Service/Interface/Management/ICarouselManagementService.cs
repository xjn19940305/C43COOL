using C43COOL.Models.Carousel.Management;
using C43COOL.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface ICarouselManagementService
    {
        /// <summary>
        /// 轮播图列表查询接口
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(CarouselQueryModel request);
        /// <summary>
        /// 新增轮播图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Create(CarouselCreateModel model);
        /// <summary>
        /// 修改轮播图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Modify(CarouselModifyModel model);
        /// <summary>
        /// 批量删除轮播图
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task Delete(string[] ids);

        /// <summary>
        /// 根据ID获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<CarouselMangementDto> Get(string Id);
    }
}
