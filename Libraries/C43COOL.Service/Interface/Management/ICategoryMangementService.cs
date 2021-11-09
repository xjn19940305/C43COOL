using C43COOL.Models.Category.Management;
using C43COOL.Models.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Interface.Management
{
    public interface ICategoryMangementService
    {
        /// <summary>
        /// 新增分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Create(CategoryCreateModel dto);
        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task Update(CategoryModifyModel dto);
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task Delete(string[] Id);
        /// <summary>
        /// 根据ID获得分类详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<CategoryDTO> Get(string Id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedViewModel> Query(CategoryRequest request);
    }
}
